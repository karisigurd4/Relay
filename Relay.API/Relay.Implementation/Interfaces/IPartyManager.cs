using Relay.Contracts;

namespace Relay.Implementation.Interfaces
{
  public interface IPartyManager
  {
    SetPartyLeaderPlayerResponse SetPartyLeaderPlayer(SetPartyLeaderPlayerRequest request);
    KickPartyPlayerResponse KickPartyPlayer(KickPartyPlayerRequest request);
    InvitePlayerToPartyResponse InvitePlayerToParty(InvitePlayerToPartyRequest request);
    GetPlayerPartyResponse GetPlayerParty(GetPlayerPartyRequest request);
    LeavePartyResponse LeaveParty(LeavePartyRequest request);
  }
}
