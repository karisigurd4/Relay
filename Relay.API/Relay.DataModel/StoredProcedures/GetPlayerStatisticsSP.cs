namespace Relay.DataModel
{
  public class GetPlayerStatisticsSPRequest
  {
    public string ExtAuthId { get; set; }
    public string ProjectId { get; set; }
    public Period Period { get; set; }
  }

  public class GetPlayerStatisticsSPResponseJson
  {
    public bool Success { get; set; }
    public string ResultsJson { get; set; }
  }

  public class GetPlayerStatisticsSPResponse
  {
    public bool Success { get; set; }
    public PlayerStatistics[] PlayerStatistics { get; set; }
  }

  public static class GetPlayerStatisticsSP
  {
    public static string Name => "[Relay].[GetPlayerStatistics]";

    public static object CreateParameters(string extAuthId, string projectId, Period period) => new
    {
      extAuthId,
      projectId,
      period = (int)period
    };
  }
}
