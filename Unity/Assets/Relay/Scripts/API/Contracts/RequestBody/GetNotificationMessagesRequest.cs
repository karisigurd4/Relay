namespace Relay.Contracts
{
  public class GetNotificationMessagesRequest
  {
    public string ApiKey { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
  }
}
