namespace Relay.DataModel
{
  using System;

  public class GetPlayerSPResponseJson
  {
    public bool Success { get; set; }
    public string PlayerJson { get; set; }
    public string PublicPlayerDataJson { get; set; }
    public string PrivatePlayerDataJson { get; set; }
  }

  public class GetPlayerSPRequest
  {
    public Guid ProjectId { get; set; }
    public int Id { get; set; }
    public string ApiKey { get; set; }
  }

  public static class GetPlayerSP
  {
    public static string Name => "[Relay].[GetPlayer]";

    public static object CreateParameters(Guid projectId, int? playerId, string playerApiKey) => new
    {
      projectId,
      playerId,
      playerApiKey
    };
  }
}
