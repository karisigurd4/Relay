namespace Relay.GameServer.Repository.Interfaces
{
  using DataModel;
  using Relay.GameServer.Core.Interfaces;

  public interface IServiceCatalogConfigurationRepository
  {
    GetServiceCatalogConfigurationSPResponse GetServiceCatalogConfiguration(IRelayUnitOfWork unitOfWork, GetServiceCatalogConfigurationSPRequest request);
  }
}
