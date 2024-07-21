namespace Relay.DataModel
{
  public class AddPartyPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class AddPartyPlayerSPRequest
  {
    public int PartyId { get; set; }
    public int PlayerId { get; set; }
  }

  public static class AddPartyPlayerSP
  {
    public static string Name => "[Relay].[AddPartyPlayer]";

    public static object CreateParameters(int partyId, int playerId) => new
    {
      partyId,
      playerId
    };
  }
}
