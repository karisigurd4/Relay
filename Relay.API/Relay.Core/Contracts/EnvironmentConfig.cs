namespace Relay.Core.Contracts
{
  public class EnvironmentConfig
  {
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public string DbConnectionString { get; set; }
    public string GameServerExecutablePath { get; set; }
    public string GameServerExecutableName { get; set; }
    public string IPAddress { get; set; }
  }
}
