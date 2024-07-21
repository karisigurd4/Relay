namespace Relay.GameServer.Repository.Interfaces
{
  using DataModel;

  public interface IGameObjectStateRepository
  {
    UpdateGameObjectStateResponse UpdateGameObjectState(UpdateGameObjectStateRequest request);
    GetGameStateResponse GetGameState(GetGameStateRequest request);
    RemoveGameObjectStateResponse RemoveGameObjectState(RemoveGameObjectStateRequest request);
    HandleClientDisconnectedResponse HandleClientDisconnected(HandleClientDisconnectedRequest request);
  }
}
