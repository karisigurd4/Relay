namespace Relay.Repository.Interfaces
{
  using Relay.Core.Interfaces;
  using Relay.DataModel;

  public interface IProjectRepository
  {
    CreateOrUpdateProjectSettingsSPResponse CreateOrUpdateProjectSettings(IRelayUnitOfWork unitOfWork, CreateOrUpdateProjectSettingsSPRequest request);
    SetProjectServiceTierSPResponse SetProjectServiceTier(IRelayUnitOfWork unitOfWork, SetProjectServiceTierSPRequest request);
    SetProjectSubscriptionSPResponse SetProjectSubscription(IRelayUnitOfWork unitOfWork, SetProjectSubscriptionSPRequest request);
    GetProjectSubscriptionSPResponse GetProjectSubscription(IRelayUnitOfWork unitOfWork, GetProjectSubscriptionSPRequest request);
  }
}
