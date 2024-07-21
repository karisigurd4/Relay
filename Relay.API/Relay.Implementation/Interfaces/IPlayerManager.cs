namespace Relay.Implementation.Interfaces
{
  using Contracts;

  public interface IPlayerManager
  {
    RegisterPlayerResponse RegisterPlayer(RegisterPlayerRequest request);
    GetPlayerResponse GetPlayer(GetPlayerRequest request);
    UpdatePlayerResponse UpdatePlayer(UpdatePlayerRequest request);
    SearchPlayersResponse SearchPlayers(SearchPlayersRequest request);
    SendPlayerFriendRequestResponse SendPlayerFriendRequest(SendPlayerFriendRequest request);
    GetPlayerFriendsListResponse GetPlayerFriendsList(GetPlayerFriendsListRequest request);
    RemovePlayerFromFriendListResponse RemovePlayerFromFriendsList(RemovePlayerFromFriendListRequest request);
    IncrementPlayerScoreResponse IncrementPlayerScore(IncrementPlayerScoreRequest request);
    SetPlayerNameResponse SetPlayerName(SetPlayerNameRequest request);
    GetPlayerStatisticsResponse GetPlayerStatistics(GetPlayerStatisticsRequest request);
  }
}
