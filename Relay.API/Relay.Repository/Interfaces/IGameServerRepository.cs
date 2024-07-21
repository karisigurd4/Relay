namespace Relay.Repository.Interfaces
{
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.GameServer.DataModel;

  public interface IGameServerRepository
  {
    AddGameServerSPResponse AddGameServer(IRelayUnitOfWork unitOfWork, AddGameServerSPRequest request);
    AddGameServerPlayerSPResponse AddGameServerPlayer(IRelayUnitOfWork unitOfWork, AddGameServerPlayerSPRequest request);
    RemoveGameServerPlayerSPResponse RemoveGameServerPlayer(IRelayUnitOfWork unitOfWork, RemoveGameServerPlayerSPRequest request);
    FindGameServerSPResponse FindGameServer(IRelayUnitOfWork unitOfWork, FindGameServerSPRequest request);
    SetGameServerStateSPResponse SetGameServerState(IRelayUnitOfWork unitOfWork, SetGameServerStateSPRequest request);
    GetAvailableGameServerPortSPResponse GetAvailableGameServerPort(IRelayUnitOfWork unitOfWork, GetAvailableGameServerPortSPRequest request);
    GetGameServerByIdSPResponse GetGameServerById(IRelayUnitOfWork unitOfWork, GetGameServerByIdSPRequest request);
    SetGameServerProcessIdSPResponse SetGameServerProcessId(IRelayUnitOfWork unitOfWork, SetGameServerProcessIdSPRequest request);
    AddGameServerOperationRequestSPResponse AddGameServerOperationRequest(IRelayUnitOfWork unitOfWork, AddGameServerOperationRequestSPRequest request);
    GetGameServerInfoByIdSPResponse GetGameServerInfoById(IRelayUnitOfWork unitOfWork, GetGameServerInfoByIdSPRequest request);
    BrowseGameServersSPResponse BrowseGameServers(IRelayUnitOfWork unitOfWork, BrowseGameServersSPRequest request);
    RegisterGameServerConfigurationSPResponse RegisterGameServerConfiguration(IRelayUnitOfWork unitOfWork, RegisterGameServerConfigurationSPRequest request);
    GetGameServerStatisticsSPResponse GetGameServerStatistics(IRelayUnitOfWork unitOfWork, GetGameServerStatisticsSPRequest request);
    GetGameServerByCodeSPResponse GetGameServerByCode(IRelayUnitOfWork unitOfWork, GetGameServerByCodeSPRequest request);
  }
}
