namespace Relay.Contracts
{
  public class RemoveGameServerPlayerRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
  }
}
