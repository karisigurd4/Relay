namespace Relay.Repository.Interfaces
{
  using DataModel;
  using Relay.Core.Interfaces;

  public interface IPartyRepository
  {
    AddPartyPlayerSPResponse AddPartyPlayer(IRelayUnitOfWork unitOfWork, AddPartyPlayerSPRequest request);
    RemovePartyPlayerSPResponse RemovePartyPlayer(IRelayUnitOfWork unitOfWork, RemovePartyPlayerSPRequest request);
    CreatePartySPResponse CreateParty(IRelayUnitOfWork unitOfWork, CreatePartySPRequest request);
    SetPartyLeaderPlayerSPResponse SetPartyLeaderPlayer(IRelayUnitOfWork unitOfWork, SetPartyLeaderPlayerSPRequest request);
    KickPartyPlayerSPResponse KickPartyPlayer(IRelayUnitOfWork unitOfWork, KickPartyPlayerSPRequest request);
    GetPlayerPartySPResponse GetPlayerParty(IRelayUnitOfWork unitOfWork, GetPlayerPartySPRequest request);
    LeavePartySPResponse LeaveParty(IRelayUnitOfWork unitOfWork, LeavePartySPRequest request);
  }
}
