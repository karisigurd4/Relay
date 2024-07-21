namespace Relay.DataModel
{
  using System;

  public class SetProjectSubscriptionSPRequest
  {
    public Guid ProjectId { get; set; }
    public string SubscriptionId { get; set; }
    public bool Active { get; set; }
  }

  public class SetProjectSubscriptionSPResponse
  {
    public bool Success { get; set; }
  }

  public static class SetProjectSubscriptionSP
  {
    public static string Name => "[Relay].[SetProjectSubscription]";

    public static object CreateParameters(Guid projectId, string subscriptionId, bool active) => new
    {
      projectId,
      subscriptionId,
      active
    };
  }
}
