namespace Relay.Implementation.Interfaces
{
  using Contracts;

  public interface IGameServerManager
  {
    FindGameServerResponse FindGameServer(FindGameServerRequest request);
    AddGameServerPlayerResponse AddGameServerPlayer(AddGameServerPlayerRequest request);
    RemoveGameServerPlayerResponse RemoveGameServerPlayer(RemoveGameServerPlayerRequest request);
    StopGameServerResponse StopGameServer(StopGameServerRequest request);
    GetGameServerInfoByIdResponse GetGameServerInfoById(GetGameServerInfoByIdRequest request);
    BrowseGameServersResponse BrowseGameServers(BrowseGameServersRequest request);
    HostGameServerResponse HostGameServer(HostGameServerRequest request);
    GetGameServerStatisticsResponse GetGameServerStatistics(GetGameServerStatisticsRequest request);
    GetGameServerByCodeResponse GetGameServerByCode(GetGameServerByCodeRequest request);
  }
}
