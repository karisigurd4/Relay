namespace Relay.Contracts
{
  public class RemovePlayerFromFriendListRequest
  {
    public string ApiKey { get; set; }
    public int RemoveFriendPlayerId { get; set; }
  }
}
