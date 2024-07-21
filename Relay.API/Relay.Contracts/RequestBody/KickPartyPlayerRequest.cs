namespace Relay.Contracts
{
  using System;

  public class KickPartyPlayerRequest
  {
    public Guid PlayerApiKey { get; set; }
    public int PartyId { get; set; }
    public int KickPlayerId { get; set; }
  }
}
