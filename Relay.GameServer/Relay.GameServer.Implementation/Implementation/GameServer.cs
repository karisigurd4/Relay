namespace Relay.GameServer.Implementation.Implementation
{
  using Relay.GameServer.Contracts;
  using Relay.GameServer.Core.Factories;
  using Relay.GameServer.Core.Types;
  using Relay.GameServer.Core.Utilities;
  using Relay.GameServer.DataModel;
  using Relay.GameServer.Implementation.Interfaces;
  using Relay.GameServer.Repository.Interfaces;
  using Riptide;
  using Riptide.Utils;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  public class GameServer : IGameServer
  {
    public static int GameServerId = 0;

    private readonly IClientConnectionManager clientConnectionManager;
    private readonly IGameObjectStateRepository gameObjectStateRepository;
    private readonly IGameServerRepository gameServerRepository;
    private readonly IGameServerStateManager gameServerStateManager;
    private readonly IServiceCatalogConfigurationRepository serviceCatalogConfigurationRepository;

    private static Server server;
    private bool running = false;

    private static List<Message> receivedGameStateMessages = new List<Message>();
    private static Stack<Message> receivedClientRpcRequests = new Stack<Message>();
    private static Dictionary<ushort, int> receivedClientPingRequests = new Dictionary<ushort, int>();
    private static Dictionary<ushort, DateTime> receivedClientPingAt = new Dictionary<ushort, DateTime>();

    private Dictionary<int, int> playerApiIdToClientId = new Dictionary<int, int>();

    private int broadcastGameStateChunkIndex = 0;
    private int broadcastClientRpcRequestChunkIndex = 0;

    private static ServiceCatalogConfiguration serviceCatalogConfiguration;

    public GameServer
    (
      IClientConnectionManager clientConnectionManager,
      IGameObjectStateRepository gameObjectStateRepository,
      IGameServerStateManager gameServerStateManager,
      IGameServerRepository gameServerRepository,
      IServiceCatalogConfigurationRepository serviceCatalogConfigurationRepository
    )
    {
      this.serviceCatalogConfigurationRepository = serviceCatalogConfigurationRepository;
      this.clientConnectionManager = clientConnectionManager;
      this.gameObjectStateRepository = gameObjectStateRepository;
      this.gameServerStateManager = gameServerStateManager;
      this.gameServerRepository = gameServerRepository;
    }

    public void Start(int gameServerId, ushort port, GameServerState gameServerStartState, Guid projectId)
    {
      GameServerId = gameServerId;

      RiptideLogger.Initialize(Console.WriteLine, Console.WriteLine, Console.WriteLine, Console.WriteLine, false);
      RiptideLogger.Log(LogType.Info, $"Started game server at (utc): {DateTime.UtcNow}");

      running = true;

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var getProjectSettingsResponse = gameServerRepository.GetProjectSettings(uw, new GetProjectSettingsSPRequest()
        {
          ProjectId = projectId
        });

        var getServiceCatalogConfigurationResponse = serviceCatalogConfigurationRepository.GetServiceCatalogConfiguration(uw, new GetServiceCatalogConfigurationSPRequest()
        {
          ProjectId = projectId
        });

        serviceCatalogConfiguration = getServiceCatalogConfigurationResponse.ServiceCatalogConfiguration;

        gameServerStateManager.Initialize(gameServerId, gameServerStartState, getProjectSettingsResponse.ProjectSettings);
      }

      ServerThread(gameServerId, port);
      UpdateUntilStop(gameServerId);
    }

    private Message CreateMessage(MessageSendMode sendMode, ushort handlerEnumId)
    {
      lock (server)
      {
        return Message.Create(sendMode, handlerEnumId);
      }
    }

    private void ServerThread(int gameServerId, ushort port)
    {
      Task.Run(() =>
      {
        server = new Server();
        server.Start(port, (ushort)serviceCatalogConfiguration.MaxPlayersPerGameServer);

        RiptideLogger.Log(LogType.Error, $"Called server.Start on port {port}");

        server.ClientConnected += (s, e) => clientConnectionManager.OnClientConnected(e, gameServerStateManager.GetGameServerState(), gameServerId, server);
        server.ClientDisconnected += (s, e) =>
        {
          DisconnectPlayer(e.Client.Id);
          clientConnectionManager.OnClientDisconnected(e.Client.Id, gameServerId, server);
        };

        RiptideLogger.Log(LogType.Info, $"Connection manager is listening for client connections on port {server.Port}");

        while (running)
        {
          try
          {
            CheckInactiveClients();
            BroadcastGameState();

            lock (server)
            {
              gameServerStateManager.Update(server);

              if (gameServerStateManager.GetGameServerState() == GameServerState.Finished)
              {
                running = false;
              }
            }
          }
          catch (Exception e)
          {
            RiptideLogger.Log(LogType.Error, e.Message);
          }

          Thread.Sleep(serviceCatalogConfiguration.BroadcastTickRate);
        }
      });
    }

    private void CheckInactiveClients()
    {
      foreach (var kv in receivedClientPingAt)
      {
        if (clientConnectionManager.GetConnectedClientIds().Contains(kv.Key) && (DateTime.UtcNow - kv.Value).Seconds > 5)
        {
          RiptideLogger.Log(LogType.Info, $"Client with id {kv.Key} is inactive. Disconnecting");
          DisconnectPlayer(kv.Key);
          clientConnectionManager.OnClientDisconnected(kv.Key, GameServerId, server);
        }
      }
    }

    private void BroadcastGameState()
    {
      lock (server)
      {
        var getGameStateResponse = gameObjectStateRepository.GetGameState(new GetGameStateRequest() { });
        if (getGameStateResponse.GameState.Length > 0)
        {
          var chunks = getGameStateResponse.GameState.ToList().ChunkBy(5);
          if (chunks.Count > 0)
          {
            for (int x = 0; x < Math.Min(chunks.Count, serviceCatalogConfiguration.GameStateUpdateChunkCount); x++)
            {
              if (broadcastClientRpcRequestChunkIndex >= chunks.Count)
              {
                broadcastClientRpcRequestChunkIndex = 0;
              }

              var chunk = chunks[broadcastClientRpcRequestChunkIndex++];

              if (chunk.Count > 0)
              {
                var m = CreateMessage(MessageSendMode.Unreliable, (ushort)GameServerMessageType.ServerGameState);
                m.AddInt(chunk.Count);
                for (int i = 0; i < chunk.Count; i++)
                {
                  chunk[i].AppendToMessage(m);
                }

                server.SendToAll(m);
              }
            }
          }
        }
      }
    }

    private void UpdateUntilStop(int gameServerId)
    {
      while (running)
      {
        if (server != null)
        {
          try
          {
            lock (server)
            {
              server.Update();
            }
          }
          catch (Exception e)
          {
            if (!string.IsNullOrWhiteSpace(e.Message))
            {
              RiptideLogger.Log(LogType.Error, e.Message);
            }
          }
        }

        HandleReceivedClientRpcRequestMessages();
        HandleReceivedGameStateMessages();

        Thread.Sleep(serviceCatalogConfiguration.ReceiveTickRate);
      }

      Thread.Sleep(100);
      StopGameServer(gameServerId);
      RiptideLogger.Log(LogType.Info, $"Game server stopped running at (utc): {DateTime.UtcNow}");
    }

    private void StopGameServer(int gameServerId)
    {
      lock (server)
      {
        server.Stop();
        RiptideLogger.Log(LogType.Info, $"Game server stopped at (utc): {DateTime.UtcNow}");
        using (var uw = RelayUnitOfWorkFactory.Create())
        {
          gameServerRepository.StopGameServer(uw, new StopGameServerSPRequest()
          {
            GameServerId = gameServerId
          });
        }
      }
    }

    [MessageHandler((ushort)GameServerMessageType.Ping)]
    public static void ReceiveClientPingRequest(ushort fromClientId, Message message)
    {
      lock (server)
      {
        var pingId = message.GetInt();

        var m = Message.Create(MessageSendMode.Reliable, (ushort)GameServerMessageType.Ping);
        m.AddInt(pingId);

        server.Send(m, fromClientId);

        if (!receivedClientPingAt.ContainsKey(fromClientId))
        {
          RiptideLogger.Log(LogType.Info, $"Adding ping entry for client {fromClientId}");
          receivedClientPingAt.Add(fromClientId, DateTime.UtcNow);
        }
        else
        {
          receivedClientPingAt[fromClientId] = DateTime.UtcNow;
        }
      }
    }

    [MessageHandler((ushort)GameServerMessageType.ClientRpcRequest)]
    public static void ReceiveClientRpcRequestMessage(ushort fromClientId, Message message)
    {
      RiptideLogger.Log(LogType.Info, $"Received Client RPC request from client {fromClientId}");
      lock (server)
      {
        receivedClientRpcRequests.Push(message);
      }
    }

    [MessageHandler((ushort)GameServerMessageType.ClientGameState)]
    public static void ReceiveClientConnectionDetailsMessage(ushort fromClientId, Message message)
    {
      lock (server)
      {
        receivedGameStateMessages.Add(message);
      }
    }

    private void DisconnectPlayer(ushort clientId)
    {
      lock (server)
      {
        gameObjectStateRepository.HandleClientDisconnected(new HandleClientDisconnectedRequest()
        {
          ClientId = clientId
        });

        if (playerApiIdToClientId.Any(x => x.Value == clientId))
        {
          var playerApiIdToClientIdEntry = playerApiIdToClientId.FirstOrDefault(x => x.Value == clientId);

          using (var uw = RelayUnitOfWorkFactory.Create())
          {
            gameServerRepository.RemoveGameServerPlayer(uw, new RemoveGameServerPlayerSPRequest()
            {
              GameServerId = GameServerId,
              PlayerId = playerApiIdToClientIdEntry.Key
            });
          }

          playerApiIdToClientId.Remove(playerApiIdToClientIdEntry.Key);
        }
      }
    }

    private void HandleReceivedGameStateMessages()
    {
      if (server == null)
      {
        return;
      }

      lock (server)
      {
        for (int i = 0; i < receivedGameStateMessages.Count; i++)
        {
          try
          {
            var count = receivedGameStateMessages[i].GetInt();

            for (int x = 0; x < count; x++)
            {
              var receivedGameState = GameObjectState.FromMessage(receivedGameStateMessages[i]);

              if (!playerApiIdToClientId.ContainsKey(receivedGameState.ApiPlayerId))
              {
                playerApiIdToClientId.Add(receivedGameState.ApiPlayerId, receivedGameState.ClientId);
                using (var uw = RelayUnitOfWorkFactory.Create())
                {
                  gameServerRepository.AddGameServerPlayer(uw, new AddGameServerPlayerSPRequest()
                  {
                    GameServerId = GameServerId,
                    PlayerId = receivedGameState.ApiPlayerId
                  });
                }
              }

              if (receivedGameState.RelayInstanceId != 0 && receivedGameState.NetworkInstanceId != 0)
              {
                gameObjectStateRepository.UpdateGameObjectState(new UpdateGameObjectStateRequest()
                {
                  GameObjectState = receivedGameState
                });
              }
            }
          }
          catch (Exception e)
          {
            RiptideLogger.Log(LogType.Error, e.Message);
          }
        }

        receivedGameStateMessages.Clear();
      }
    }

    private void HandleReceivedClientRpcRequestMessages()
    {
      if (server == null)
      {
        return;
      }

      lock (server)
      {
        if (receivedClientRpcRequests.Count > 0)
        {
          var clientRpcBatch = new List<Message>();
          for (int i = 0; i < Math.Min(receivedClientRpcRequests.Count, serviceCatalogConfiguration.ClientRpcUpdateChunkCount); i++)
          {
            clientRpcBatch.Add(receivedClientRpcRequests.Pop());
          }

          foreach (var rpcRequest in clientRpcBatch)
          {
            try
            {
              var count = rpcRequest.GetInt();
              for (int i = 0; i < count; i++)
              {
                RiptideLogger.Log(LogType.Info, "Handling Rpc");
                var m = CreateMessage(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientRpcRequest);

                var clientRpcRequest = ClientRpcRequest.FromMessage(rpcRequest);

                clientRpcRequest.AppendToMessage(m);

                if (clientRpcRequest.Broadcast)
                {
                  server.SendToAll(m);
                }
                else
                {
                  server.Send(m, clientRpcRequest.ReceiverClientId);
                }
              }
            }
            catch (Exception e)
            {
              RiptideLogger.Log(LogType.Error, "Something went wrong in handling rpc's");
            }
          }
        }
      }
    }
  }
}
