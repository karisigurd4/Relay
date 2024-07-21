namespace Relay.DataModel
{
  public class RemovePartyPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class RemovePartyPlayerSPRequest
  {
    public int PartyId { get; set; }
    public int PlayerId { get; set; }
  }

  public static class RemovePartyPlayerSP
  {
    public static string Name => "[Relay].[RemovePartyPlayer]";

    public static object CreateParameters(int partyId, int playerId) => new
    {
      partyId,
      playerId
    };
  }
}
