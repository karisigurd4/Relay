using AutoMapper;
using Relay.Core.Interfaces;
using Relay.DataModel;
using Relay.Repository.Interfaces;

namespace Relay.Repository.Implementation
{
  public class ProjectRepository : IProjectRepository
  {
    private readonly IMapper mapper;

    public ProjectRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public CreateOrUpdateProjectSettingsSPResponse CreateOrUpdateProjectSettings(IRelayUnitOfWork unitOfWork, CreateOrUpdateProjectSettingsSPRequest request)
    {
      return unitOfWork.ExecuteSP<CreateOrUpdateProjectSettingsSPResponse>
      (
        CreateOrUpdateProjectSettingsSP.Name,
        CreateOrUpdateProjectSettingsSP.CreateParameters
        (
          request.ExtAuthId,
          request.ProjectId,
          request.ServiceCatalogId,
          request.LobbySystemType,
          request.GameModesJsonData,
          request.MaximumPlayerCapacity,
          request.EnablePreGameLobby,
          request.RestrictJoiningToPreGameLobby,
          request.MaximumLobbyTimeInSeconds,
          request.EnableMatchTimeLimit,
          request.MaximumActiveMatchTimeInSeconds,
          request.EnableLevelBasedMatchmaking,
          request.MatchmakingPlayerDataKey,
          request.MatchmakingOptimalRange
        )
      );
    }

    public GetProjectSubscriptionSPResponse GetProjectSubscription(IRelayUnitOfWork unitOfWork, GetProjectSubscriptionSPRequest request)
    {
      return mapper.Map<GetProjectSubscriptionSPResponse>
      (
        unitOfWork.ExecuteSP<GetProjectSubscriptionSPResponseJson>
        (
          GetProjectSubscriptionSP.Name,
          GetProjectSubscriptionSP.CreateParameters(request.ProjectId)
        )
      );
    }

    public SetProjectServiceTierSPResponse SetProjectServiceTier(IRelayUnitOfWork unitOfWork, SetProjectServiceTierSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetProjectServiceTierSPResponse>
      (
        SetProjectServiceTierSP.Name,
        SetProjectServiceTierSP.CreateParameters(request.ProjectId, request.ServiceTier)
      );
    }

    public SetProjectSubscriptionSPResponse SetProjectSubscription(IRelayUnitOfWork unitOfWork, SetProjectSubscriptionSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetProjectSubscriptionSPResponse>
      (
        SetProjectSubscriptionSP.Name,
        SetProjectSubscriptionSP.CreateParameters(request.ProjectId, request.SubscriptionId, request.Active)
      );
    }
  }
}
