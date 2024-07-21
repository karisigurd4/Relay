namespace Relay.Contracts
{
  public class BrowseGameServersResponse : BaseResponse
  {
    public GameServerListEntry[] GameServerList { get; set; }
    public int TotalCount { get; set; }
  }
}
