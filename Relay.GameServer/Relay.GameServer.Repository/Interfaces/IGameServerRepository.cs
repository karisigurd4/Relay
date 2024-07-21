namespace Relay.GameServer.Repository.Interfaces
{
  using Relay.GameServer.Core.Interfaces;
  using Relay.GameServer.DataModel;

  public interface IGameServerRepository
  {
    AddGameServerPlayerSPResponse AddGameServerPlayer(IRelayUnitOfWork unitOfWork, AddGameServerPlayerSPRequest request);
    RemoveGameServerPlayerSPResponse RemoveGameServerPlayer(IRelayUnitOfWork unitOfWork, RemoveGameServerPlayerSPRequest request);
    PollGameServerOperationsSPResponse PollGameServerOperations(IRelayUnitOfWork relayUnitOfWork, PollGameServerOperationsSPRequest request);
    SetGameServerProcessIdSPResponse SetGameServerProcessId(IRelayUnitOfWork unitOfWork, SetGameServerProcessIdSPRequest request);
    StopGameServerSPResponse StopGameServer(IRelayUnitOfWork unitOfWork, StopGameServerSPRequest request);
    SetGameServerStateSPResponse SetGameServerState(IRelayUnitOfWork unitOfWork, SetGameServerStateSPRequest request);
    GetProjectSettingsSPResponse GetProjectSettings(IRelayUnitOfWork unitOfWork, GetProjectSettingsSPRequest request);
    GetActiveGameServersSPResponse GetActiveGameServers(IRelayUnitOfWork unitOfWork, GetActiveGameServersSPRequest request);
  }
}
