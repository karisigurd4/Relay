namespace Relay.Repository.Implementation
{
  using global::AutoMapper;
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.Repository.Interfaces;
  using System;

  public class PlayerRepository : IPlayerRepository
  {
    private readonly IMapper mapper;

    public PlayerRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public AddPlayerFriendSPResponse AddPlayerFriend(IRelayUnitOfWork unitOfWork, AddPlayerFriendSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddPlayerFriendSPResponse>(AddPlayerFriendSP.Name, AddPlayerFriendSP.CreateParameters(request.Player1Id, request.Player2Id));
    }

    public GetPlayerSPResponse GetPlayer(IRelayUnitOfWork unitOfWork, GetPlayerSPRequest request)
    {
      var getPlayerResponseJson = unitOfWork.ExecuteSP<GetPlayerSPResponseJson>(GetPlayerSP.Name, GetPlayerSP.CreateParameters(request.ProjectId, !string.IsNullOrWhiteSpace(request.ApiKey) ? (int?)null : request.Id, string.IsNullOrWhiteSpace(request.ApiKey) ? null : request.ApiKey));

      return mapper.Map<GetPlayerSPResponse>(getPlayerResponseJson);
    }

    public GetPlayerFriendsListSPResponse GetPlayerFriendsList(IRelayUnitOfWork unitOfWork, GetPlayerFriendsListSPRequest request)
    {
      var getPlayerFriendsListResponseJson = unitOfWork.ExecuteSP<GetPlayerFriendsListSPResponseJson>(GetPlayerFriendsListSP.Name, GetPlayerFriendsListSP.CreateParameters(request.ApiKey));

      return mapper.Map<GetPlayerFriendsListSPResponse>(getPlayerFriendsListResponseJson);
    }

    public GetPlayerStatisticsSPResponse GetPlayerStatistics(IRelayUnitOfWork unitOfWork, GetPlayerStatisticsSPRequest request)
    {
      var jsonResponse = unitOfWork.ExecuteSP<GetPlayerStatisticsSPResponseJson>(GetPlayerStatisticsSP.Name, GetPlayerStatisticsSP.CreateParameters(request.ExtAuthId, request.ProjectId, request.Period));

      return mapper.Map<GetPlayerStatisticsSPResponse>(jsonResponse);
    }

    public IncrementPlayerScoreSPResponse IncrementPlayerScore(IRelayUnitOfWork unitOfWork, IncrementPlayerScoreSPRequest request)
    {
      return unitOfWork.ExecuteSP<IncrementPlayerScoreSPResponse>(IncrementPlayerScoreSP.Name, IncrementPlayerScoreSP.CreateParameters(request.GameServerId, request.PlayerId, request.ScoreType.GetHashCode(), request.ScoreType, request.Amount));
    }

    public RegisterPlayerSPResponse RegisterPlayer(IRelayUnitOfWork unitOfWork, RegisterPlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<RegisterPlayerSPResponse>(RegisterPlayerSP.Name, RegisterPlayerSP.CreateParameters(request.ProjectId, request.Name));
    }

    public RemovePlayerFriendSPResponse RemovePlayerFriend(IRelayUnitOfWork unitOfWork, RemovePlayerFriendSPRequest request)
    {
      return unitOfWork.ExecuteSP<RemovePlayerFriendSPResponse>(RemovePlayerFriendSP.Name, RemovePlayerFriendSP.CreateParameters(request.ApiKey, request.RemoveFriendPlayerId));
    }

    public SearchPlayersSPResponse SearchPlayers(IRelayUnitOfWork unitOfWork, SearchPlayersSPRequest request)
    {
      // Unity text inputs add zero-width whitespace UTC characters to strings. This is just baffling.
      request.Query = request.Query.Replace("\u200B", "");

      var searchPlayersSPResponseJson = unitOfWork.ExecuteSP<SearchPlayersSPResponseJson>(SearchPlayersSP.Name, SearchPlayersSP.CreateParameters(request.ProjectId, request.Query, 0, 10));

      return mapper.Map<SearchPlayersSPResponse>(searchPlayersSPResponseJson);
    }

    public SetPlayerNameSPResponse SetPlayerName(IRelayUnitOfWork unitOfWork, SetPlayerNameSPRequest request)
    {
      return unitOfWork.ExecuteSP<SetPlayerNameSPResponse>(SetPlayerNameSP.Name, SetPlayerNameSP.CreateParameters(Guid.Parse(request.PlayerApiKey), request.NewPlayerName));
    }

    public UpdatePlayerSPResponse UpdatePlayer(IRelayUnitOfWork unitOfWork, UpdatePlayerSPRequest request)
    {
      return unitOfWork.ExecuteSP<UpdatePlayerSPResponse>(UpdatePlayerSP.Name, UpdatePlayerSP.CreateParameters(request.ApiKey, request.Key, request.Value, request.Public));
    }
  }
}
