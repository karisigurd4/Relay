using Newtonsoft.Json;
using Relay.Core.Contracts;
using RestSharp;
using System.Linq;
using System.Security.Claims;


namespace Relay.Core.Utilities
{
  public static class TokenUtils
  {
    public static AccessToken GetAccessToken()
    {
      var client = new RestClient("https://auth.bittershark.com/");
      var request = new RestRequest("oauth/token", Method.Post);
      request.AddHeader("content-type", "application/json");
      request.AddParameter("application/json", "{\"client_id\":\"\",\"client_secret\":\"\",\"audience\":\"/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
      RestResponse response = client.Execute(request);
      var responseContent = response.Content.ToString();
      return JsonConvert.DeserializeObject<AccessToken>(responseContent);
    }

    public static string GetUserId(ClaimsPrincipal claimsPrincipal) =>
      claimsPrincipal
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
  }
}
