namespace Relay.IntegrationTest.ApiClients
{
  using Relay.Contracts;
  using Relay.IntegrationTest.Utils;
  using System.Net.Http;

  public static class GameServerClient
  {
    private static HttpClient httpClient = new HttpClient();

    public static FindGameServerResponse FindGameServer(FindGameServerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.GameServerRoute}/find");
      var response = HttpClientUtils.Post(httpClient, Constants.GameServerRoute + "/find", request);
      return HttpClientUtils.GetContent<FindGameServerResponse>(response);
    }

    public static AddGameServerPlayerResponse AddGameServerPlayer(AddGameServerPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.GameServerRoute}/gameServerId/player");
      var response = HttpClientUtils.Post(httpClient, Constants.GameServerRoute + "/" + request.GameServerId, request);
      return HttpClientUtils.GetContent<AddGameServerPlayerResponse>(response);
    }

    public static AddGameServerPlayerResponse RemoveGameServerPlayer(AddGameServerPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.GameServerRoute}/gameServerId/player");
      var response = HttpClientUtils.Post(httpClient, Constants.GameServerRoute + "/" + request.GameServerId + "/player/remove", request);
      return HttpClientUtils.GetContent<AddGameServerPlayerResponse>(response);
    }
  }
}
