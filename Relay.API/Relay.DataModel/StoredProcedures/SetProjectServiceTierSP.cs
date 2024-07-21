namespace Relay.DataModel
{
  using System;

  public class SetProjectServiceTierSPRequest
  {
    public Guid ProjectId { get; set; }
    public int ServiceTier { get; set; }
  }

  public class SetProjectServiceTierSPResponse
  {
    public bool Success { get; set; }
  }

  public static class SetProjectServiceTierSP
  {
    public static string Name => "[Relay].[SetProjectServiceTier]";

    public static object CreateParameters(Guid projectId, int serviceTier) => new
    {
      projectId,
      serviceTier
    };
  }
}
