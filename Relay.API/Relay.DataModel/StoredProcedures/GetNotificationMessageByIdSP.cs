namespace Relay.DataModel
{
  using System;

  public class GetNotificationMessageByIdSPRequest
  {
    public int Id { get; set; }
  }

  public class GetNotificationMessageByIdSPResponse
  {
    public int Id { get; set; }
    public int ToPlayerId { get; set; }
    public int ReferenceId { get; set; }
    public string Data { get; set; }
    public NotificationMessageType Type { get; set; }
    public DateTime SentDateTime { get; set; }
  }

  public static class GetNotificationMessageByIdSP
  {
    public static string Name => "[Relay].[GetNotificationMessageById]";

    public static object CreateParameters(int id) => new
    {
      id
    };
  }
}
