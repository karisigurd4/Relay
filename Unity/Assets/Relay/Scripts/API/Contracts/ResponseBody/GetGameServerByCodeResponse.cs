namespace Relay.Contracts
{
  public class GetGameServerByCodeResponse : BaseResponse
  {
    public GameServerListEntry GameServer { get; set; }
  }
}
