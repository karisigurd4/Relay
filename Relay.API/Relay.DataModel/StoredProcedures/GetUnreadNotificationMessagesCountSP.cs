namespace Relay.DataModel
{
  using System;

  public class GetUnreadNotificationMessagesCountSPResponse
  {
    public bool Success { get; set; }
    public int Count { get; set; }
  }

  public class GetUnreadNotificationMessagesCountSPRequest
  {
    public Guid PlayerApiKey { get; set; }
  }

  public static class GetUnreadNotificationMessagesCountSP
  {
    public static string Name => "[Relay].[GetUnreadNotificationMessagesCount]";

    public static object CreateParameters(Guid playerApiKey) => new
    {
      playerApiKey
    };
  }
}
