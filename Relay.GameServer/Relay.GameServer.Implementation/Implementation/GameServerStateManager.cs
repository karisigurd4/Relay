using Relay.GameServer.Contracts;
using Relay.GameServer.Core.Factories;
using Relay.GameServer.Core.Types;
using Relay.GameServer.DataModel;
using Relay.GameServer.Implementation.Interfaces;
using Relay.GameServer.Repository.Interfaces;
using Riptide;
using Riptide.Utils;
using System;

namespace Relay.GameServer.Implementation.Implementation
{
  public class GameServerStateManager : IGameServerStateManager
  {
    private const int DefaultLobbyWaitTime = 10;
    private const bool DefaultLobbyEnable = true;
    private const int DefaultActiveStateTime = 180;
    private const bool DefaultActiveTimeEnable = true;

    private readonly IClientConnectionManager clientConnectionManager;
    private readonly IGameServerRepository gameServerRepository;

    private GameServerState gameServerState;
    private DateTime currentStateStarted;
    private int gameServerId;
    private bool checkConnectionsClose = false;

    private ProjectSettings projectSettings;

    public GameServerStateManager(IClientConnectionManager clientConnectionManager, IGameServerRepository gameServerRepository)
    {
      this.clientConnectionManager = clientConnectionManager;
      this.gameServerRepository = gameServerRepository;
    }

    public GameServerState GetGameServerState()
    {
      return gameServerState;
    }

    public void Initialize(int gameServerId, GameServerState startingState, ProjectSettings projectSettings)
    {
      if (startingState == GameServerState.Active)
      {
        checkConnectionsClose = false;
      }

      this.projectSettings = projectSettings;

      this.gameServerId = gameServerId;
      gameServerState = startingState;
      RiptideLogger.Log(LogType.Info, $"Game state initialized to {startingState}");
      currentStateStarted = DateTime.UtcNow;
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        gameServerRepository.SetGameServerState(uw, new DataModel.SetGameServerStateSPRequest()
        {
          GameServerId = gameServerId,
          State = startingState.ToString()
        });
      }
    }

    public void Update(Server server)
    {
      switch (gameServerState)
      {
        case GameServerState.Waiting: UpdateWaitingState(server); break;
        case GameServerState.Lobby: UpdateLobbyState(server); break;
        case GameServerState.Active: UpdateActiveState(server); break;
        case GameServerState.Finished: break;
      }

      if (checkConnectionsClose && clientConnectionManager.GetConnectedClientIds().Length == 0 && gameServerState != GameServerState.Waiting)
      {
        gameServerState = GameServerState.Finished;
        RiptideLogger.Log(LogType.Info, $"All clients disconnected. Setting state to finished");
      }
    }

    private void UpdateActiveState(Server server)
    {
      int activeTimeLimit = DefaultActiveStateTime;

      if (projectSettings != null && !projectSettings.EnableMatchTimeLimit)
      {
        return;
      }

      if (projectSettings != null && projectSettings.EnableMatchTimeLimit)
      {
        activeTimeLimit = projectSettings.MaximumActiveMatchTimeInSeconds;
      }

      if ((DateTime.UtcNow - currentStateStarted).Seconds > activeTimeLimit)
      {
        RiptideLogger.Log(LogType.Info, $"Switching from Active state to Finished state");
        gameServerState = GameServerState.Finished;
        currentStateStarted = DateTime.UtcNow;
        UpdateDatabase();
        SendUpdateMessage(server);
      }
    }

    private void UpdateLobbyState(Server server)
    {
      int waitTime = DefaultLobbyWaitTime;

      if (projectSettings != null)
      {
        if (!projectSettings.EnablePreGameLobby)
        {
          gameServerState = GameServerState.Active;
          currentStateStarted = DateTime.UtcNow;
          UpdateDatabase();
          SendUpdateMessage(server);
        }

        waitTime = projectSettings.MaximumLobbyTimeInSeconds;
      }

      if ((DateTime.UtcNow - currentStateStarted).Seconds > waitTime)
      {
        RiptideLogger.Log(LogType.Info, $"Switching from Lobby state to Active state");
        gameServerState = GameServerState.Active;
        currentStateStarted = DateTime.UtcNow;
        UpdateDatabase();
        SendUpdateMessage(server);
      }
    }

    private void UpdateWaitingState(Server server)
    {
      if ((DateTime.UtcNow - currentStateStarted).Seconds > 20)
      {
        gameServerState = GameServerState.Finished;
        UpdateDatabase();
        SendUpdateMessage(server);
      }

      if (clientConnectionManager.GetConnectedClientIds().Length > 0)
      {
        if (projectSettings == null && DefaultLobbyEnable)
        {
          RiptideLogger.Log(LogType.Info, $"Switching from Waiting state to Lobby state");
          gameServerState = GameServerState.Lobby;
          checkConnectionsClose = true;
        }
        else
        {
          if (projectSettings.EnablePreGameLobby)
          {
            RiptideLogger.Log(LogType.Info, $"Switching from Waiting state to Lobby state");
            gameServerState = GameServerState.Lobby;
            checkConnectionsClose = true;
          }
          else
          {
            RiptideLogger.Log(LogType.Info, $"Switching from Waiting state to Active state");
            gameServerState = GameServerState.Active;
            checkConnectionsClose = true;
          }
        }

        currentStateStarted = DateTime.UtcNow;
        UpdateDatabase();
        SendUpdateMessage(server);
      }
    }

    private void UpdateDatabase()
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        gameServerRepository.SetGameServerState(uw, new DataModel.SetGameServerStateSPRequest()
        {
          GameServerId = this.gameServerId,
          State = gameServerState.ToString()
        });
      }
    }

    private void SendUpdateMessage(Server server)
    {
      var m = Message.Create(MessageSendMode.Reliable, (ushort)GameServerMessageType.GameServerStateUpdated);
      m.AddInt((int)gameServerState);
      server.SendToAll(m);
    }
  }
}
