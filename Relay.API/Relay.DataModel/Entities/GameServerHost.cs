namespace Relay.DataModel
{
  public class GameServerHost
  {
    public string HostName { get; set; }
    public float CpuUsage { get; set; }
    public float MemoryUsage { get; set; }
    public GameServerHostStatus Status { get; set; }
  }
}
