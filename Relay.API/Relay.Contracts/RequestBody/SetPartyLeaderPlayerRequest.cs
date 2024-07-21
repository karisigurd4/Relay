namespace Relay.Contracts
{
  using System;

  public class SetPartyLeaderPlayerRequest
  {
    public Guid PlayerApiKey { get; set; }
    public int PartyId { get; set; }
    public int NewLeaderPlayerId { get; set; }
  }
}
