namespace Relay.DataModel
{
  public class AddGameServerSPResponse
  {
    public int GameServerId { get; set; }
    public bool Success { get; set; }
  }

  public class AddGameServerSPRequest
  {
    public string HostName { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public string ProjectId { get; set; }
    public string Tag { get; set; }
  }

  public static class AddGameServerSP
  {
    public static string Name => "[Relay].[AddGameServer]";

    public static object CreateParameters(string hostName, string ipAddress, int port, string projectId, string tag) => new
    {
      hostName,
      ipAddress,
      port,
      projectId,
      tag
    };
  }
}
