using System;

namespace Relay.GameServer.DataModel
{
  public class GetServiceCatalogConfigurationSPRequest
  {
    public Guid ProjectId { get; set; }
  }

  public class GetServiceCatalogConfigurationSPResponseJson
  {
    public string ServiceCatalogConfigurationJson;
    public bool Success { get; set; }
  }

  public class GetServiceCatalogConfigurationSPResponse
  {
    public ServiceCatalogConfiguration ServiceCatalogConfiguration;
    public bool Success { get; set; }
  }

  public static class GetServiceCatalogConfigurationSP
  {
    public static string Name => "[Relay].[GetServiceCatalogConfiguration]";

    public static object CreateParameters(string projectId) => new
    {
      projectId
    };
  }
}
