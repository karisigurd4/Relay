namespace Relay.IntegrationTest.ApiClients
{
  using Relay.Contracts;
  using Relay.IntegrationTest.Utils;
  using System.Net.Http;

  public static class PlayerClient
  {
    private static HttpClient httpClient = new HttpClient();

    public static RegisterPlayerResponse RegisterPlayer(RegisterPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/register");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/register", request);
      return HttpClientUtils.GetContent<RegisterPlayerResponse>(response);
    }


    public static GetPlayerResponse GetPlayer(GetPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/", request);
      return HttpClientUtils.GetContent<GetPlayerResponse>(response);
    }

    public static UpdatePlayerResponse UpdatePlayer(UpdatePlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/update");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/update", request);
      return HttpClientUtils.GetContent<UpdatePlayerResponse>(response);
    }

    public static SearchPlayersResponse SearchPlayers(SearchPlayersRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/search");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/search", request);
      return HttpClientUtils.GetContent<SearchPlayersResponse>(response);
    }

    public static SendPlayerFriendRequestResponse SendPlayerFriendRequest(SendPlayerFriendRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/friends/request");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/friends/request", request);
      return HttpClientUtils.GetContent<SendPlayerFriendRequestResponse>(response);
    }

    public static RemovePlayerFromFriendListResponse RemovePlayerFromFriendList(RemovePlayerFromFriendListRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/friends/remove");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/friends/remove", request);
      return HttpClientUtils.GetContent<RemovePlayerFromFriendListResponse>(response);
    }

    public static GetPlayerFriendsListResponse GetPlayerFriendsList(GetPlayerFriendsListRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PlayerRoute}/friends/list");
      var response = HttpClientUtils.Post(httpClient, Constants.PlayerRoute + "/friends/list", request);
      return HttpClientUtils.GetContent<GetPlayerFriendsListResponse>(response);
    }
  }
}
