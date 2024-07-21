namespace Relay.DataModel
{
  using System;

  public class KickPartyPlayerSPRequest
  {
    public Guid LeaderPlayerApiKey { get; set; }
    public int PartyId { get; set; }
    public int KickPlayerId { get; set; }
  }

  public class KickPartyPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public static class KickPartyPlayerSP
  {
    public static string Name => "[Relay].[KickPartyPlayer]";

    public static object CreateParameters(Guid leaderPlayerApiKey, int partyId, int kickPlayerId) => new
    {
      leaderPlayerApiKey,
      partyId,
      kickPlayerId
    };
  }
}
