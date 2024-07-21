using Relay.Toolkit.Networking;
using Riptide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerClient : MonoBehaviour
  {
    [Header("Configuration")]
    public bool ConnectToLocal = false;
    public bool TestInactivity = false;

    private const int MaxMessagesInBatch = 5;
    private const int PingTickRatio = 10;

    public static bool IsConnected = false;
    public static ushort ClientId;

    private static object Lock = new object();
    private static int gameServerId;
    private static int playerApiId;
    private static bool receivedConnectionDetails;
    private static List<ushort> connectedClientIds = new List<ushort>();
    private static List<ushort> disconnectedClientIds = new List<ushort>();
    private Task serverClientTask;
    private static int pingTickCounter = 1;
    private static DateTime pingSentTime;
    private static int pingId = 0;
    private static bool receivedPingResponse = true;
    private static bool receivedDisconnection = false;

    private Client client;
    private bool clientRunning = true;

    private void Awake()
    {
      receivedConnectionDetails = false;
      IsConnected = false;
    }

    private void Start()
    {
      if (ConnectToLocal)
      {
        ConnectToGameServer("192.168.0.4", 11002);
      }
    }

    private Message CreateMessage(MessageSendMode sendMode, ushort handlerEnumId)
    {
      lock (client)
      {
        return Message.Create(sendMode, handlerEnumId);
      }
    }

    public void ConnectToGameServer(string ipAddress, int port)
    {
      playerApiId = RelayPlayerManager.Instance.GetPlayerId();

      Debug.Log($"Relay: Connecting");

      MessageQueues.ClearAllQueues();
      GameStateRepository.Clear();
      connectedClientIds.Clear();
      disconnectedClientIds.Clear();
      RpcMethodCache.Clear();

      receivedConnectionDetails = false;
      clientRunning = true;
      receivedDisconnection = false;

      serverClientTask = Task.Run(() =>
      {
        client = new Client();
        //client.Connected += (s, e) => OnConnected();
        client.ClientConnected += (s, e) => OnClientConnected(s, e);
        client.Disconnected += (s, e) => OnDisconnected();
        client.ClientDisconnected += (s, e) => OnClientDisconnected(s, e);
        if (!client.Connect($"{ipAddress}:{port}"))
        {
          Debug.LogError($"Could not connect to {ipAddress}:{port}");
          return;
        }

        Debug.Log($"Relay: Connected.");

        MessageBusManager.Instance.Publish(new GameServerConnectionEstablishedMessage() { });

        while (clientRunning && !receivedDisconnection)
        {
          if (!TestInactivity)
          {
            if (!receivedConnectionDetails)
            {
              Thread.Sleep(24);

              continue;
            }

            if (pingTickCounter > PingTickRatio && receivedPingResponse)
            {
              lock (Lock)
              {
                pingTickCounter = 0;
                receivedPingResponse = false;
                var m = CreateMessage(MessageSendMode.Reliable, (ushort)GameServerMessageType.Ping);
                m.AddInt(pingId);
                client.Send(m);
                pingSentTime = DateTime.UtcNow;
              }
            }

            pingTickCounter += 1;

            BroadcastLocalGameState();
            BroadcastPendingClientRpcRequests();

            Thread.Sleep(24);
          }
        }

        clientRunning = false;

        if (client.IsConnected)
        {
          client.Disconnect();
          IsConnected = false;
        }
      });
    }

    private void OnClientConnected(object s, ClientConnectedEventArgs e)
    {
      lock (Lock)
      {
        if (!connectedClientIds.Contains(e.Id))
        {
          connectedClientIds.Add(e.Id);
        }
      }
    }

    private void OnClientDisconnected(object s, ClientDisconnectedEventArgs e)
    {
      lock (Lock)
      {
        if (connectedClientIds.Contains(e.Id))
        {
          connectedClientIds.Remove(e.Id);
        }

        disconnectedClientIds.Add(e.Id);

        MessageBusManager.Instance.Publish(new ClientDisconnectedMessage()
        {
          ClientId = e.Id
        });
      }
    }

    private void FixedUpdate()
    {
      if (client != null && clientRunning)
      {
        lock (Lock)
        {
          try
          {
            client.Update();
          }
          catch
          {
            MessageQueues.PendingClientRpcRequestQueue.PopAll();
          }

          HandleReceivedClientRpcRequests();
        }
      }
    }

    public static bool IsLowestIdClient()
    {
      if (connectedClientIds == null || connectedClientIds.Count == 0)
      {
        return false;
      }

      return connectedClientIds.Where(x => x != 0).Min() == ClientId;
    }

    public static bool IsDisconnectedClient(ushort clientId)
    {
      return disconnectedClientIds.Contains(clientId);
    }


    public static bool IsConnectedClient(ushort clientId)
    {
      return connectedClientIds.Contains(clientId);
    }

    public static ushort GetLowestIdClient()
    {
      if (connectedClientIds.Count == 0)
      {
        return 0;
      }

      return connectedClientIds.Where(x => x != 0).Min();
    }

    private void HandleReceivedClientRpcRequests()
    {
      lock (Lock)
      {
        if (!IsConnected)
        {
          return;
        }

        var pendingClientRpcRequests = MessageQueues.ReceivedClientRpcRequestQueue.PopAll();

        for (int i = 0; i < pendingClientRpcRequests.Length; i++)
        {
          RpcMethodCache.ExecuteMethod
          (
            pendingClientRpcRequests[i].RelayGameObjectId,
            pendingClientRpcRequests[i].MethodId,
            pendingClientRpcRequests[i].Parameters
          );
        }
      }
    }

    private void BroadcastLocalGameState()
    {
      if (TestInactivity)
      {
        return;
      }

      lock (Lock)
      {
        var localGameState = GameStateRepository.GetState(true);

        if (localGameState.Length > 0)
        {
          var chunks = localGameState.ToList().ChunkBy(5);

          foreach (var chunk in chunks)
          {
            var m = CreateMessage(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientGameState);
            m.AddInt(chunk.Count);

            for (int i = 0; i < chunk.Count; i++)
            {
              var gameState = new GameObjectState()
              {
                ApiPlayerId = playerApiId,
                ClientId = ClientId,
                RelayInstanceId = chunk[i].RelayInstanceId,
                NetworkInstanceId = chunk[i].NetworkInstanceId,
                State = chunk[i].GetState()
              };

              gameState.AppendToMessage(m, (m, o) => MessageSerializer.WriteObjectToMessage(m, o));
            }

            client.Send(m);
          }
        }
      }
    }

    private void BroadcastPendingClientRpcRequests()
    {
      if (TestInactivity)
      {
        return;
      }

      lock (Lock)
      {
        var pendingClientRpcBatch = new List<ClientRpcRequest>();
        for (int i = 0; i < Mathf.Min(MessageQueues.PendingClientRpcRequestQueue.Count, 5); i++)
        {
          pendingClientRpcBatch.Add(MessageQueues.PendingClientRpcRequestQueue.Pop());
        }

        if (pendingClientRpcBatch.Count > 0)
        {
          for (int i = 0; i < pendingClientRpcBatch.Count; i++)
          {
            var m = CreateMessage(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientRpcRequest);
            m.AddInt(1);
            pendingClientRpcBatch[i].AppendToMessage(m, (m, o) => MessageSerializer.WriteParametersToMessage(m, o));

            client.Send(m);
          }
        }
      }
    }

    void OnDisconnected()
    {
    }

    private void StopClient()
    {
      lock (Lock)
      {
        clientRunning = false;
        if (client != null)
        {
          client.Disconnect();
        }
      }
    }

    public void OnApplicationQuit()
    {
      StopClient();
    }

    [RuntimeInitializeOnLoadMethod]
    public void OnStart()
    {
      Application.quitting += () =>
      {
        StopClient();
      };
    }

    [MessageHandler((ushort)GameServerMessageType.ClientDisconnected)]
    private static void ReceiveClientDisconnectionInfo(Message message)
    {
      lock (Lock)
      {
        ushort clientId = message.GetUShort();

        if (clientId == ClientId)
        {
          Debug.Log($"Diconnected: Make sure to enable 'Run in background' in your player options if unexpected.");
          receivedDisconnection = true;
        }

        if (!disconnectedClientIds.Contains(clientId))
        {
          disconnectedClientIds.Add(clientId);
        }
      }
    }

    [MessageHandler((ushort)GameServerMessageType.Ping)]
    private static void ReceivePingResponse(Message message)
    {
      lock (Lock)
      {
        var id = message.GetInt();

        if (id == pingId)
        {
          receivedPingResponse = true;

          MessageBusManager.Instance.Publish(new PingUpdatedMessage()
          {
            Ping = (DateTime.UtcNow - pingSentTime).Milliseconds / 2
          });
        }
        message.Release();
      }
    }

    [MessageHandler((ushort)GameServerMessageType.ClientConnectionDetails)]
    private static void ReceiveClientConnectionDetailsMessage(Message message)
    {
      lock (Lock)
      {
        ClientId = message.GetUShort();
        if (!connectedClientIds.Contains(ClientId))
        {
          connectedClientIds.Add(ClientId);
        }
        gameServerId = message.GetInt();
        var gameServerState = (GameServerState)message.GetInt();
        MessageBusManager.Instance.Publish(new GameServerStateUpdatedMessage()
        {
          GameServerState = gameServerState
        });
        receivedConnectionDetails = true;
        GameStateRepository.NetworkInstanceIdIndex = ((int)ClientId) * 30000;
        IsConnected = true;
      }
    }

    [MessageHandler((ushort)GameServerMessageType.ServerGameState)]
    private static void ReceiveServerGameStateMessage(Message message)
    {
      lock (Lock)
      {
        var count = message.GetInt();
        for (int i = 0; i < count; i++)
        {
          try
          {
            var gameObjectState = GameObjectState.FromMessage(message, o => MessageSerializer.ReadObjectFromMessage(o));
            if (IsDisconnectedClient(gameObjectState.ClientId))
            {
              return;
            }

            if (!connectedClientIds.Contains(gameObjectState.ClientId))
            {
              connectedClientIds.Add(gameObjectState.ClientId);
            }

            var relayGameObject = GameStateRepository.GetRelayGameObjectByNetworkInstanceId(gameObjectState.NetworkInstanceId);
            if (relayGameObject == null)
            {
              MessageBusManager.Instance.Publish(new GameObjectStateAddedMessage()
              {
                GameObjectState = gameObjectState
              });
            }
            else
            {
              MessageBusManager.Instance.Publish(new GameObjectStateUpdatedMessage()
              {
                GameObjectState = gameObjectState
              });
            }
          }
          catch
          {
          }
        }
      }
    }

    [MessageHandler((ushort)GameServerMessageType.ClientRpcRequest)]
    private static void ReceiveClientRpcRequest(Message message)
    {
      lock (Lock)
      {
        MessageQueues.ReceivedClientRpcRequestQueue.Push(ClientRpcRequest.FromMessage(message, m => MessageSerializer.ReadParametersFromMessage(m)));
      }
    }

    [MessageHandler((ushort)GameServerMessageType.GameServerStateUpdated)]
    private static void ReceiveGameServerStateUpdated(Message message)
    {
      lock (Lock)
      {
        var state = (GameServerState)message.GetInt();
        MessageBusManager.Instance.Publish(new GameServerStateUpdatedMessage()
        {
          GameServerState = state
        });
      }
    }
  }
}
