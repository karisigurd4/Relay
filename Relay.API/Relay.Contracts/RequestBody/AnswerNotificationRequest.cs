namespace Relay.Contracts
{
  public class AnswerNotificationRequest
  {
    public string ProjectId { get; set; }
    public string PlayerApiKey { get; set; }
    public int NotificationMessageId { get; set; }
    public Answer Answer { get; set; }
  }
}
