using Newtonsoft.Json;
using Relay.GameServer.Core.Interfaces;
using Relay.GameServer.DataModel;
using Relay.GameServer.Repository.Interfaces;

namespace Relay.GameServer.Repository.Implementation
{
  public class ServiceCatalogConfigurationRepository : IServiceCatalogConfigurationRepository
  {
    public GetServiceCatalogConfigurationSPResponse GetServiceCatalogConfiguration(IRelayUnitOfWork unitOfWork, GetServiceCatalogConfigurationSPRequest request)
    {
      var jsonResponse = unitOfWork.ExecuteSP<GetServiceCatalogConfigurationSPResponseJson>(GetServiceCatalogConfigurationSP.Name, GetServiceCatalogConfigurationSP.CreateParameters(request.ProjectId.ToString()));

      return new GetServiceCatalogConfigurationSPResponse()
      {
        Success = jsonResponse.Success,
        ServiceCatalogConfiguration = !string.IsNullOrWhiteSpace(jsonResponse.ServiceCatalogConfigurationJson) ? JsonConvert.DeserializeObject<ServiceCatalogConfiguration>(jsonResponse.ServiceCatalogConfigurationJson) : null
      };
    }
  }
}
