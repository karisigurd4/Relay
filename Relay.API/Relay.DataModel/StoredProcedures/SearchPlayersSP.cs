using System;

namespace Relay.DataModel
{
  public class SearchPlayersSPResponseJson
  {
    public bool Success { get; set; }
    public string PlayersJson { get; set; }
    public int TotalMatches { get; set; }
  }

  public class SearchPlayersSPRequest
  {
    public Guid ProjectId { get; set; }
    public string Query { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
  }

  public static class SearchPlayersSP
  {
    public static string Name => "[Relay].[SearchPlayers]";

    public static object CreateParameters(Guid projectId, string query, int offset, int count) => new
    {
      projectId,
      query,
      offset,
      count
    };
  }
}
