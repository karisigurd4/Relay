namespace Relay.GameServer.Repository.Implementation
{
  using Newtonsoft.Json;
  using Relay.GameServer.Core.Interfaces;
  using Relay.GameServer.DataModel;
  using Relay.GameServer.Repository.Interfaces;

  public class GameServerRepository : IGameServerRepository
  {
    public PollGameServerOperationsSPResponse PollGameServerOperations(IRelayUnitOfWork relayUnitOfWork, PollGameServerOperationsSPRequest request)
    {
      var jsonResponse = relayUnitOfWork.ExecuteSP<PollGameServerOperationsSPResponseJson>
      (
        PollGameServerOperationsSP.Name,
        PollGameServerOperationsSP.CreateParameters(request.HostName)
      );

      if (jsonResponse == null || jsonResponse.OperationRequestsJson == null)
      {
        return new PollGameServerOperationsSPResponse()
        {
          Success = true,
          OperationRequests = new GameServerOperationRequest[] { }
        };
      }

      return new PollGameServerOperationsSPResponse()
      {
        OperationRequests = JsonConvert.DeserializeObject<GameServerOperationRequest[]>(jsonResponse.OperationRequestsJson),
        Success = jsonResponse.Success
      };
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

    public StopGameServerSPResponse StopGameServer(IRelayUnitOfWork unitOfWork, StopGameServerSPRequest request)
    {
      return unitOfWork.ExecuteSP<StopGameServerSPResponse>
      (
        StopGameServerSP.Name,
        StopGameServerSP.CreateParameters
        (
          request.GameServerId
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

    public AddGameServerPlayerSPResponse AddGameServerPlayer(IRelayUnitOfWork unitOfWork, AddGameServerPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddGameServerPlayerSPResponse>
      (
        AddGameServerPlayerSP.Name,
        AddGameServerPlayerSP.CreateParameters
        (
          request.GameServerId,
          request.PlayerId
        )
      );
    }

    public GetProjectSettingsSPResponse GetProjectSettings(IRelayUnitOfWork unitOfWork, GetProjectSettingsSPRequest request)
    {
      var getProjectResponseJson = unitOfWork.ExecuteSP<GetProjectSettingsSPResponseJson>
      (
        GetProjectSettingsSP.Name,
        GetProjectSettingsSP.CreateParameters(request.ProjectId)
      );

      if (!getProjectResponseJson.Success)
      {
        return new GetProjectSettingsSPResponse()
        {
          Success = false
        };
      }

      return new GetProjectSettingsSPResponse()
      {
        Success = getProjectResponseJson.Success,
        ProjectSettings = !string.IsNullOrWhiteSpace(getProjectResponseJson.ProjectSettingsJson) ? JsonConvert.DeserializeObject<ProjectSettings>(getProjectResponseJson.ProjectSettingsJson) : null
      };
    }

    public GetActiveGameServersSPResponse GetActiveGameServers(IRelayUnitOfWork unitOfWork, GetActiveGameServersSPRequest request)
    {
      var responseJson = unitOfWork.ExecuteSP<GetActiveGameServersSPResponseJson>
      (
        GetActiveGameServersSP.Name,
        GetActiveGameServersSP.CreateParameters(request.HostName)
      );

      if (!responseJson.Success)
      {
        return new GetActiveGameServersSPResponse()
        {
          Success = false
        };
      }

      return new GetActiveGameServersSPResponse()
      {
        Success = responseJson.Success,
        GameServers = !string.IsNullOrWhiteSpace(responseJson.GameServersJson) ? JsonConvert.DeserializeObject<GameServer[]>(responseJson.GameServersJson) : new GameServer[] { }
      };
    }
  }
}
