namespace Relay.Implementation
{
  using Relay.Contracts;

  public static class AnswerNotificationResponseFactory
  {
    public static AnswerNotificationResponse Create(bool success, string message) => new AnswerNotificationResponse()
    {
      Success = success,
      Message = message
    };

    public static AnswerNotificationResponse Create() => new AnswerNotificationResponse() { Success = true };
  }
}
