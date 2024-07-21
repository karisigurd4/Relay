namespace Relay.DataModel
{
  public class HideNotificationSPRequest
  {
    public int Id { get; set; }
  }

  public class HideNotificationSPResponse
  {
    public bool Success { get; set; }
  }

  public static class HideNotificationSP
  {
    public static string Name => "[Relay].[HideNotification]";

    public static object CreateParameters(int id) => new
    {
      id
    };
  }
}
