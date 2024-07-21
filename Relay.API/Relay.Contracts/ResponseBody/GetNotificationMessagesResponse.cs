namespace Relay.Contracts
{
  public class GetNotificationMessagesResponse : BaseResponse
  {
    public NotificationMessage[] NotificationMessages { get; set; }
  }
}
