namespace Relay.Contracts
{
  public class HostGameServerRequest
  {
    public string ProjectId { get; set; }
    public string ServerName { get; set; }
    public bool IsPrivate { get; set; }
    public string Code { get; set; }
    public string Mode { get; set; }
    public int MaxPlayers { get; set; }
  }
}
