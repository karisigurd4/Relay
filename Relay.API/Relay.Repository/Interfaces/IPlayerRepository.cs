namespace Relay.Repository.Interfaces
{
  using DataModel;
  using Relay.Core.Interfaces;

  public interface IPlayerRepository
  {
    GetPlayerSPResponse GetPlayer(IRelayUnitOfWork unitOfWork, GetPlayerSPRequest request);
    RegisterPlayerSPResponse RegisterPlayer(IRelayUnitOfWork unitOfWork, RegisterPlayerSPRequest request);
    SearchPlayersSPResponse SearchPlayers(IRelayUnitOfWork unitOfWork, SearchPlayersSPRequest request);
    UpdatePlayerSPResponse UpdatePlayer(IRelayUnitOfWork unitOfWork, UpdatePlayerSPRequest request);
    AddPlayerFriendSPResponse AddPlayerFriend(IRelayUnitOfWork unitOfWork, AddPlayerFriendSPRequest request);
    RemovePlayerFriendSPResponse RemovePlayerFriend(IRelayUnitOfWork unitOfWork, RemovePlayerFriendSPRequest request);
    GetPlayerFriendsListSPResponse GetPlayerFriendsList(IRelayUnitOfWork unitOfWork, GetPlayerFriendsListSPRequest request);
    IncrementPlayerScoreSPResponse IncrementPlayerScore(IRelayUnitOfWork unitOfWork, IncrementPlayerScoreSPRequest request);
    SetPlayerNameSPResponse SetPlayerName(IRelayUnitOfWork unitOfWork, SetPlayerNameSPRequest request);
    GetPlayerStatisticsSPResponse GetPlayerStatistics(IRelayUnitOfWork unitOfWork, GetPlayerStatisticsSPRequest request);
  }
}
