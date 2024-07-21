namespace Relay.DataModel
{
  using System;

  public class LeavePartySPRequest
  {
    public string PlayerApiKey { get; set; }
  }

  public class LeavePartySPResponse
  {
    public bool Success { get; set; }
  }

  public static class LeavePartySP
  {
    public static string Name => "[Relay].[LeaveParty]";

    public static object CreateParameters(Guid playerApiKey) => new
    {
      playerApiKey
    };
  }
}
