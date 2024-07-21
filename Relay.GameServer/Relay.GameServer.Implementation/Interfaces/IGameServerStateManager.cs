namespace Relay.GameServer.Implementation.Interfaces
{
  using Relay.GameServer.Core.Types;
  using Relay.GameServer.DataModel;
  using Riptide;

  public interface IGameServerStateManager
  {
    void Initialize(int gameServerId, GameServerState startingState, ProjectSettings projectSettings);
    void Update(Server server);
    GameServerState GetGameServerState();
  }
}
