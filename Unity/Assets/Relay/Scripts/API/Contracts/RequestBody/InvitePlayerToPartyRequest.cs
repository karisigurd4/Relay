namespace Relay.Contracts
{
  public class InvitePlayerToPartyRequest
  {
    public string ProjectId { get; set; }
    public string FromPlayerApiKey { get; set; }
    public int ToPlayerId { get; set; }
  }
}
