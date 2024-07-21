namespace Relay.Contracts
{
  public class HostGameServerResponse : BaseResponse
  {
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public int GameServerId { get; set; }
  }
}
