namespace Relay.Repository.Implementation
{
  using global::AutoMapper;
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.GameServer.DataModel;
  using Relay.Repository.Interfaces;

  public class GameServerRepository : IGameServerRepository
  {
    private readonly IMapper mapper;

    public GameServerRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public AddGameServerSPResponse AddGameServer(IRelayUnitOfWork unitOfWork, AddGameServerSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddGameServerSPResponse>
      (
        AddGameServerSP.Name,
        AddGameServerSP.CreateParameters
        (
          request.HostName,
          request.IPAddress,
          request.Port,
          request.ProjectId,
          request.Tag
        )
      );
    }

    public AddGameServerPlayerSPResponse AddGameServerPlayer(IRelayUnitOfWork unitOfWork, AddGameServerPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddGameServerPlayerSPResponse>
      (
        AddGameServerPlayerSP.Name,
        AddGameServerPlayerSP.CreateParameters
        (
          request.GameServerHost,
          request.GameServerId,
          request.PlayerId
        )
      );
    }

    public AddGameServerOperationRequestSPResponse AddGameServerOperationRequest(IRelayUnitOfWork unitOfWork, AddGameServerOperationRequestSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddGameServerOperationRequestSPResponse>
      (
        AddGameServerOperationRequestSP.Name,
        AddGameServerOperationRequestSP.CreateParameters
        (
          request.GameServerHost,
          request.Operation.ToString(),
          request.Port,
          request.GameServerId,
          request.ProjectId
        )
      );
    }

    public FindGameServerSPResponse FindGameServer(IRelayUnitOfWork unitOfWork, FindGameServerSPRequest request)
    {
      var findGameServerResponseJson = unitOfWork.ExecuteSP<FindGameServerSPResponseJson>
      (
        FindGameServerSP.Name,
        FindGameServerSP.CreateParameters(request.ProjectId, request.Tag)
      );

      return mapper.Map<FindGameServerSPResponse>(findGameServerResponseJson);
    }

    public GetAvailableGameServerPortSPResponse GetAvailableGameServerPort(IRelayUnitOfWork unitOfWork, GetAvailableGameServerPortSPRequest request)
    {
      return unitOfWork.ExecuteSP<GetAvailableGameServerPortSPResponse>
      (
        GetAvailableGameServerPortSP.Name,
        GetAvailableGameServerPortSP.CreateParameters(request.GameServerHost)
      );
    }

    public GetGameServerByIdSPResponse GetGameServerById(IRelayUnitOfWork unitOfWork, GetGameServerByIdSPRequest request)
    {
      var getGameServerByIdResponseJson = unitOfWork.ExecuteSP<GetGameServerByIdSPResponseJson>
      (
        GetGameServerByIdSP.Name,
        GetGameServerByIdSP.CreateParameters
        (
          request.GameServerId
        )
      );

      return mapper.Map<GetGameServerByIdSPResponse>(getGameServerByIdResponseJson);
    }

    public RemoveGameServerPlayerSPResponse RemoveGameServerPlayer(IRelayUnitOfWork unitOfWork, RemoveGameServerPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<RemoveGameServerPlayerSPResponse>
      (
        RemoveGameServerPlayerSP.Name,
        RemoveGameServerPlayerSP.CreateParameters
        (
          request.GameServerId,
          request.PlayerId
        )
      );
    }

    public SetGameServerProcessIdSPResponse SetGameServerProcessId(IRelayUnitOfWork unitOfWork, SetGameServerProcessIdSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetGameServerProcessIdSPResponse>
      (
        SetGameServerProcessIdSP.Name,
        SetGameServerProcessIdSP.CreateParameters
        (
          request.GameServerId,
          request.ProcessId
        )
      );
    }

    public SetGameServerStateSPResponse SetGameServerState(IRelayUnitOfWork unitOfWork, SetGameServerStateSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetGameServerStateSPResponse>
      (
        SetGameServerStateSP.Name,
        SetGameServerStateSP.CreateParameters
        (
          request.GameServerId,
          request.State.ToString()
        )
      );
    }

    public GetGameServerInfoByIdSPResponse GetGameServerInfoById(IRelayUnitOfWork unitOfWork, GetGameServerInfoByIdSPRequest request)
    {
      var getGameServerInfoByIdResponseJson = unitOfWork.ExecuteSP<GetGameServerInfoByIdSPResponseJson>
      (
        GetGameServerInfoByIdSP.Name,
        GetGameServerInfoByIdSP.CreateParameters
        (
          request.GameServerId
        )
      );

      return mapper.Map<GetGameServerInfoByIdSPResponse>(getGameServerInfoByIdResponseJson);
    }

    public BrowseGameServersSPResponse BrowseGameServers(IRelayUnitOfWork unitOfWork, BrowseGameServersSPRequest request)
    {
      var browseGameServersJsonResponse = unitOfWork.ExecuteSP<BrowseGameServersSPResponseJson>
      (
        BrowseGameServersSP.Name,
        BrowseGameServersSP.CreateParameters(request.ProjectId, request.HideFull, request.HidePrivate, request.Page, request.PageSize, request.OrderBy, request.OrderDirection)
      );

      return mapper.Map<BrowseGameServersSPResponse>(browseGameServersJsonResponse);
    }

    public RegisterGameServerConfigurationSPResponse RegisterGameServerConfiguration(IRelayUnitOfWork unitOfWork, RegisterGameServerConfigurationSPRequest request)
    {
      return unitOfWork.ExecuteSP<RegisterGameServerConfigurationSPResponse>
      (
        RegisterGameServerConfigurationSP.Name,
         RegisterGameServerConfigurationSP.CreateParameters(request.GameServerId, request.ServerName, request.IsPrivate, request.Code, request.Mode, request.MaxPlayers)
      );
    }

    public GetGameServerStatisticsSPResponse GetGameServerStatistics(IRelayUnitOfWork unitOfWork, GetGameServerStatisticsSPRequest request)
    {
      var jsonResponse = unitOfWork.ExecuteSP<GetGameServerStatisticsSPResponseJson>
      (
        GetGameServerStatisticsSP.Name,
        GetGameServerStatisticsSP.CreateParameters(request.ExtAuthId, request.ProjectId, request.Period)
      );

      return mapper.Map<GetGameServerStatisticsSPResponse>
      (
        jsonResponse
      );
    }

    public GetGameServerByCodeSPResponse GetGameServerByCode(IRelayUnitOfWork unitOfWork, GetGameServerByCodeSPRequest request)
    {
      var jsonResponse = unitOfWork.ExecuteSP<GetGameServerByCodeSPResponseJson>
      (
        GetGameServerByCodeSP.Name,
        GetGameServerByCodeSP.CreateParameters(request.ProjectId, request.Code)
      );

      return mapper.Map<GetGameServerByCodeSPResponse>
      (
        jsonResponse
      );
    }
  }
}
