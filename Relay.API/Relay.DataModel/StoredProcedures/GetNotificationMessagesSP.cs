namespace Relay.DataModel
{
  using System;

  public class GetNotificationMessagesSPResponseJson
  {
    public bool Success { get; set; }
    public string NotificationMessagesJson { get; set; }
  }

  public class GetNotificationMessagesSPRequest
  {
    public Guid ApiKey { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
  }

  public static class GetNotificationMessagesSP
  {
    public static string Name => "[Relay].[GetNotificationMessages]";

    public static object CreateParameters(Guid apikey, int offset, int count) => new
    {
      apikey,
      offset,
      count
    };
  }
}
