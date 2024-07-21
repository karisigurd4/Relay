namespace Relay.GameServer.DataModel
{
  public class UpdateGameServerHostInfoSPRequest
  {
    public string HostName { get; set; }
    public float CpuUsage { get; set; }
    public float MemoryUsage { get; set; }
  }

  public class UpdateGameServerHostInfoSPResponse
  {
    public bool Success { get; set; }
  }

  public static class UpdateGameServerHostInfoSP
  {
    public static string Name => "[Relay].[UpdateGameServerHostInfo]";

    public static object CreateParameters(string hostName, float cpuUsage, float memoryUsage) => new
    {
      hostName,
      cpuUsage,
      memoryUsage
    };
  }
}
