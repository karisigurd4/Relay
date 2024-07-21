using Relay.Toolkit.Networking;

namespace BitterShark.Relay
{
  public class GameServerStateUpdatedMessageConsumer : MessageBusConsumer<GameServerStateUpdatedMessage>
  {
    public RelaySceneConfiguration LobbyUISceneconfiguration;

    public override void OnConsumeMessage(GameServerStateUpdatedMessage message)
    {
      switch (message.GameServerState)
      {
        case GameServerState.Waiting:
          break;
        case GameServerState.Lobby:
          break;
        case GameServerState.Active:
          RelaySceneManager.Instance.UnloadScenesBySceneConfiguration(LobbyUISceneconfiguration);
          break;
        case GameServerState.Finished:
          break;
      }
    }
  }
}