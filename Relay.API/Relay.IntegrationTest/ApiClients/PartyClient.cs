namespace Relay.IntegrationTest.ApiClients
{
  using Relay.Contracts;
  using Relay.IntegrationTest.Utils;
  using System.Net.Http;

  public static class PartyClient
  {
    private static HttpClient httpClient = new HttpClient();

    public static SetPartyLeaderPlayerResponse SetPartyLeaderPlayer(SetPartyLeaderPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PartyRoute}/{request.PartyId}/leader");
      var response = HttpClientUtils.Post(httpClient, Constants.PartyRoute + "/" + request.PartyId + "/leader", request);
      return HttpClientUtils.GetContent<SetPartyLeaderPlayerResponse>(response);
    }

    public static KickPartyPlayerResponse KickPartyPlayer(KickPartyPlayerRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PartyRoute}/{request.PartyId}/kick");
      var response = HttpClientUtils.Post(httpClient, Constants.PartyRoute + "/" + request.PartyId + "/kick", request);
      return HttpClientUtils.GetContent<KickPartyPlayerResponse>(response);
    }

    public static InvitePlayerToPartyResponse InvitePlayerToParty(InvitePlayerToPartyRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PartyRoute}/invite");
      var response = HttpClientUtils.Post(httpClient, Constants.PartyRoute + "/invite", request);
      return HttpClientUtils.GetContent<InvitePlayerToPartyResponse>(response);
    }

    public static GetPlayerPartyResponse GetPlayerParty(GetPlayerPartyRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.PartyRoute}/");
      var response = HttpClientUtils.Post(httpClient, Constants.PartyRoute + "/", request);
      return HttpClientUtils.GetContent<GetPlayerPartyResponse>(response);
    }
  }
}
