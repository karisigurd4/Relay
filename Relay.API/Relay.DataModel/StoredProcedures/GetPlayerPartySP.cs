namespace Relay.DataModel
{
  public class GetPlayerPartySPRequest
  {
    public int PlayerId { get; set; }
  }

  public class GetPlayerPartySPResponseJson
  {
    public bool Success { get; set; }
    public string PartyJson { get; set; }
  }

  public class GetPlayerPartySPResponse
  {
    public bool Success { get; set; }
    public Party[] Party { get; set; }
  }

  public static class GetPlayerPartySP
  {
    public static string Name => "[Relay].[GetPlayerParty]";

    public static object CreateParameters(int playerId) => new
    {
      playerId
    };
  }
}
