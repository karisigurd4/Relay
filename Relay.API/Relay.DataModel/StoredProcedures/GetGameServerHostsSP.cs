namespace Relay.DataModel
{
  public class GetGameServerHostsSPRequest
  {
  }

  public class GetGameServerHostsSPResponseJson
  {
    public bool Success { get; set; }
    public string HostsJson { get; set; }
  }

  public class GetGameServerHostsSPResponse
  {
    public bool Success { get; set; }
    public GameServerHost[] Hosts { get; set; }
  }

  public static class GetGameServerHostsSP
  {
    public static string Name => "[Relay].[GetGameServerHosts]";

    public static object CreateParameters() => new
    {
    };
  }
}
