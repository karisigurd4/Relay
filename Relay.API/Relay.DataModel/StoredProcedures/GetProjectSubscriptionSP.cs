namespace Relay.DataModel
{
  using System;

  public class GetProjectSubscriptionSPRequest
  {
    public Guid ProjectId { get; set; }
  }

  public class GetProjectSubscriptionSPResponseJson
  {
    public string ProjectSubscriptionJson { get; set; }
    public bool Success { get; set; }
  }

  public class GetProjectSubscriptionSPResponse
  {
    public ProjectSubsciption ProjectSubsciption { get; set; }
    public bool Success { get; set; }
  }

  public static class GetProjectSubscriptionSP
  {
    public static string Name => "[Relay].[GetProjectSubscription]";

    public static object CreateParameters(Guid projectId) => new
    {
      projectId
    };
  }
}
