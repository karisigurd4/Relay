namespace Relay.Implementation.Interfaces
{
  using Contracts;

  public interface INotificationMessageManager
  {
    GetNotificationMessagesResponse GetNotificationMessages(GetNotificationMessagesRequest request);
    GetUnreadNotificationMessagesCountResponse GetUnreadNotificationMessagesCount(GetUnreadNotificationMessagesCountRequest request);
    AnswerNotificationResponse AnswerNotification(AnswerNotificationRequest request);
  }
}
