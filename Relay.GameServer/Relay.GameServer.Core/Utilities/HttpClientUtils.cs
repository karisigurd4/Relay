using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Relay.GameServer.Core.Utilities
{
  public static class HttpClientUtils
  {
    public static string ApiRoute = ConfigurationReader.GetGameServerConfiguration().ApiHostname;

    public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public static StringContent Serialize(object o) => new StringContent(JsonConvert.SerializeObject(o, SerializerSettings), Encoding.UTF8, "application/json");

    public static HttpResponseMessage Post(HttpClient httpClient, string route, object request) => httpClient.PostAsync(ApiRoute + route, HttpClientUtils.Serialize(request)).GetAwaiter().GetResult();
    public static HttpResponseMessage Get(HttpClient httpClient, string route) => httpClient.GetAsync(ApiRoute + route).GetAwaiter().GetResult();

    public static T GetContent<T>(HttpResponseMessage httpResponseMessage) => JsonConvert.DeserializeObject<T>(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult(), SerializerSettings);
  }
}
