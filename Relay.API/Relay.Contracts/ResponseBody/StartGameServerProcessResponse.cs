namespace Relay.Contracts
{
  public class StartGameServerProcessResponse : BaseResponse
  {
    public int GameServerId { get; set; }
    public int ProcessId { get; set; }
    public int Port { get; set; }
    public string IPAddress { get; set; }
  }
}
