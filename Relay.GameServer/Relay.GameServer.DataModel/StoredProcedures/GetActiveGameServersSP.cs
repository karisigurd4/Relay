namespace Relay.GameServer.DataModel
{
  public class GetActiveGameServersSPRequest
  {
    public string HostName { get; set; }
  }

  public class GetActiveGameServersSPResponseJson
  {
    public bool Success { get; set; }
    public string GameServersJson { get; set; }
  }

  public class GetActiveGameServersSPResponse
  {
    public bool Success { get; set; }
    public GameServer[] GameServers { get; set; }
  }

  public static class GetActiveGameServersSP
  {
    public static string Name => "[Relay].[GetActiveGameServers]";

    public static object CreateParameters(string hostName) => new
    {
      hostName
    };
  }
}
