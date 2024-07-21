namespace Relay.Contracts
{
  public class Party
  {
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public int PartyId { get; set; }
    public bool IsPartyLeader { get; set; }
    public bool InGameServer { get; set; }
    public int InGameServerId { get; set; }
    public string InGameServerIPAddress { get; set; }
    public int InGameServerPort { get; set; }
  }
}
