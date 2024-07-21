namespace Relay.GameServer.Core.Contracts
{
  public class GameServerConfiguration
  {
    public string ApiHostname { get; set; }
    public string DbConnectionString { get; set; }
    public string GameServerExecutablePath { get; set; }
    public string GameServerExecutableName { get; set; }
    public int GameStateUpdateChunkCount { get; set; }
    public int ClientRpcUpdateChunkCount { get; set; }
  }
}
