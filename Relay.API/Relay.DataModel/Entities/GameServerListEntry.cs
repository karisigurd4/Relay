namespace Relay.DataModel
{
  public class GameServerListEntry
  {
    public int GameServerId { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public bool Private { get; set; }
    public string Mode { get; set; }
    public int MaxPlayerCapacity { get; set; }
    public int CurrentPlayerCount { get; set; }
    public string GameServerName { get; set; }
    public string GameServerState { get; set; }
  }
}
