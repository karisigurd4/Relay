namespace Relay.DataModel
{
  public class GetAvailableGameServerPortSPRequest
  {
    public string GameServerHost { get; set; }
  }

  public class GetAvailableGameServerPortSPResponse
  {
    public int PortNumber { get; set; }
  }

  public static class GetAvailableGameServerPortSP
  {
    public static string Name => "[Relay].[GetAvailableGameServerPort]";

    public static object CreateParameters(string gameServerHost) => new
    {
      gameServerHost
    };
  }
}
