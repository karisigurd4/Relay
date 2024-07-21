using System;

namespace Relay.DataModel
{
  public class GetProjectPlayersSPRequest
  {
    public Guid ProjectId { get; set; }
  }

  public class GetProjectPlayersSPResponseJson
  {
    public string PlayersJson { get; set; }
    public int TotalPlayerCount { get; set; }
    public bool Success { get; set; }
  }


  public class GetProjectPlayersSPResponseIntermediary
  {
    public PlayerViewJson PlayersJson { get; set; }
    public int TotalPlayerCount { get; set; }
    public bool Success { get; set; }
  }

  public class GetProjectPlayersSPResponse
  {
    public PlayerView[] Players { get; set; }
    public int TotalPlayerCount { get; set; }
    public bool Success { get; set; }
  }

  public static class GetProjectPlayersSP
  {
    public static string Name => "[Relay].[GetProjectPlayers]";

    public static object CreateParameters(Guid projectId) => new
    {
      projectId
    };
  }
}
