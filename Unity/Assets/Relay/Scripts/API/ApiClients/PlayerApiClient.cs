using Relay.Contracts;
using System;
using UnityEngine;

namespace BitterShark.Relay
{
  public class PlayerApiClient : MonoBehaviour
  {
    private static PlayerApiClient _instance;
    public static PlayerApiClient Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;
    }

    public void RegisterPlayer(string projectId, string playerName, Action<RegisterPlayerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/register", new RegisterPlayerRequest()
      {
        ProjectId = projectId,
        Name = playerName
      }, responseCallback));
    }

    public void GetPlayer(string projectId, int playerId, string playerApiKey, Action<GetPlayerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/", new GetPlayerRequest()
      {
        ProjectId = projectId,
        ApiKey = playerApiKey,
        Id = playerId
      }, responseCallback));
    }

    public void UpdatePlayer(string playerApiKey, string key, string value, bool isPublic, Action<UpdatePlayerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/update", new UpdatePlayerRequest()
      {
        ApiKey = playerApiKey,
        Key = key,
        Public = isPublic,
        Value = value
      }, responseCallback));
    }

    public void SearchPlayers(string projectId, string query, Action<SearchPlayersResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/search", new SearchPlayersRequest()
      {
        ProjectId = projectId,
        Query = query
      }, responseCallback));
    }

    public void SendPlayerFriendRequest(string projectId, string fromPlayerApiKey, int toPlayerId, Action<SendPlayerFriendRequestResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/friends/request", new SendPlayerFriendRequest()
      {
        ProjectId = projectId,
        FromPlayerApiKey = fromPlayerApiKey,
        ToPlayerId = toPlayerId
      }, responseCallback));
    }

    public void RemovePlayerFromFriendList(string playerApiKey, int playerToRemove, Action<RemovePlayerFromFriendListResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/friends/remove", new RemovePlayerFromFriendListRequest()
      {
        ApiKey = (playerApiKey),
        RemoveFriendPlayerId = playerToRemove
      }, responseCallback));
    }

    public void GetPlayerFriendsList(string playerApiKey, Action<GetPlayerFriendsListResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/friends/list", new GetPlayerFriendsListRequest()
      {
        ApiKey = (playerApiKey)
      }, responseCallback));
    }

    public void IncrementPlayerScore(int gameServerId, int playerId, string scoreType, int amount, Action<IncrementPlayerScoreResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/score", new IncrementPlayerScoreRequest()
      {
        GameServerId = gameServerId,
        PlayerId = playerId,
        ScoreType = scoreType,
        Amount = amount
      }, responseCallback));
    }

    public void SetPlayerName(string playerApiKey, string newPlayerName, Action<SetPlayerNameResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PlayerApiRoute + "/name", new SetPlayerNameRequest()
      {
        PlayerApiKey = playerApiKey,
        NewPlayerName = newPlayerName
      }, responseCallback));
    }
  }
}
