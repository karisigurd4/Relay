namespace Relay.Repository.Implementation
{
  using global::AutoMapper;
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.Repository.Interfaces;
  using System;

  public class PartyRepository : IPartyRepository
  {
    private readonly IMapper mapper;

    public PartyRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public AddPartyPlayerSPResponse AddPartyPlayer(IRelayUnitOfWork unitOfWork, AddPartyPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddPartyPlayerSPResponse>(AddPartyPlayerSP.Name, AddPartyPlayerSP.CreateParameters(request.PartyId, request.PlayerId));
    }

    public CreatePartySPResponse CreateParty(IRelayUnitOfWork unitOfWork, CreatePartySPRequest request)
    {
      return unitOfWork.ExecuteSP<CreatePartySPResponse>(CreatePartySP.Name, CreatePartySP.CreateParameters(request.PartyLeaderPlayerId));
    }

    public GetPlayerPartySPResponse GetPlayerParty(IRelayUnitOfWork unitOfWork, GetPlayerPartySPRequest request)
    {
      var responseJson = unitOfWork.ExecuteSP<GetPlayerPartySPResponseJson>(GetPlayerPartySP.Name, GetPlayerPartySP.CreateParameters(request.PlayerId));

      return mapper.Map<GetPlayerPartySPResponse>(responseJson);
    }

    public KickPartyPlayerSPResponse KickPartyPlayer(IRelayUnitOfWork unitOfWork, KickPartyPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<KickPartyPlayerSPResponse>(KickPartyPlayerSP.Name, KickPartyPlayerSP.CreateParameters(request.LeaderPlayerApiKey, request.PartyId, request.KickPlayerId));
    }

    public LeavePartySPResponse LeaveParty(IRelayUnitOfWork unitOfWork, LeavePartySPRequest request)
    {
      return unitOfWork.ExecuteSP<LeavePartySPResponse>(LeavePartySP.Name, LeavePartySP.CreateParameters(Guid.Parse(request.PlayerApiKey)));
    }

    public RemovePartyPlayerSPResponse RemovePartyPlayer(IRelayUnitOfWork unitOfWork, RemovePartyPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<RemovePartyPlayerSPResponse>(RemovePartyPlayerSP.Name, RemovePartyPlayerSP.CreateParameters(request.PartyId, request.PlayerId));
    }

    public SetPartyLeaderPlayerSPResponse SetPartyLeaderPlayer(IRelayUnitOfWork unitOfWork, SetPartyLeaderPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetPartyLeaderPlayerSPResponse>(SetPartyLeaderPlayerSP.Name, SetPartyLeaderPlayerSP.CreateParameters(request.LeaderPlayerApiKey, request.PartyId, request.NewLeaderPlayerId));
    }
  }
}
