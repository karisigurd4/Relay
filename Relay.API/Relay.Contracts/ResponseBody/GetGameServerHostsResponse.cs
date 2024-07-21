namespace Relay.Contracts
{
  public class GetGameServerHostsResponse : BaseResponse
  {
    public GameServerHost[] Hosts { get; set; }
  }
}
