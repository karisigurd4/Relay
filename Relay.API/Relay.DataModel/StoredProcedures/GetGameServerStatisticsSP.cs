namespace Relay.DataModel
{
  public class GetGameServerStatisticsSPRequest
  {
    public string ExtAuthId { get; set; }
    public string ProjectId { get; set; }
    public Period Period { get; set; }
  }

  public class GetGameServerStatisticsSPResponseJson
  {
    public bool Success { get; set; }
    public string ResultsJson { get; set; }
  }

  public class GetGameServerStatisticsSPResponse
  {
    public bool Success { get; set; }
    public GameServerStatistics[] GameServerStatistics { get; set; }
  }

  public static class GetGameServerStatisticsSP
  {
    public static string Name => "[Relay].[GetGameServerStatistics]";

    public static object CreateParameters(string extAuthId, string projectId, Period period) => new
    {
      extAuthId,
      projectId,
      period = (int)period
    };
  }
}
