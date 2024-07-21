namespace Relay.Contracts
{
  public class SendPlayerFriendRequest
  {
    public string ProjectId { get; set; }
    public string FromPlayerApiKey { get; set; }
    public int ToPlayerId { get; set; }
  }
}
