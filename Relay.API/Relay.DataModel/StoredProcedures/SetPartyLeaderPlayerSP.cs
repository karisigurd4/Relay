namespace Relay.DataModel
{
  using System;

  public class SetPartyLeaderPlayerSPRequest
  {
    public Guid LeaderPlayerApiKey { get; set; }
    public int PartyId { get; set; }
    public int NewLeaderPlayerId { get; set; }
  }

  public class SetPartyLeaderPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public static class SetPartyLeaderPlayerSP
  {
    public static string Name => "[Relay].[SetPartyLeaderPlayer]";

    public static object CreateParameters(Guid leaderPlayerApiKey, int partyId, int newLeaderPlayerId) => new
    {
      leaderPlayerApiKey,
      partyId,
      newLeaderPlayerId
    };
  }
}
