namespace Relay.DataModel
{
  public class AddNotificationMessageSPResponse
  {
    public bool Success { get; set; }
  }

  public class AddNotificationMessageSPRequest
  {
    public int ToPlayerId { get; set; }
    public int ReferenceId { get; set; }
    public NotificationMessageType Type { get; set; }
    public string Data { get; set; }
  }

  public static class AddNotificationMessageSP
  {
    public static string Name => "[Relay].[AddNotificationMessage]";

    public static object CreateParameters(int toPlayerId, int referenceId, string type, string data) => new
    {
      toPlayerId,
      referenceId,
      type,
      data
    };
  }
}
