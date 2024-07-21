namespace Relay.IntegrationTest.Utils
{
  using Newtonsoft.Json;
  using System.Net.Http;
  using System.Text;

  public static class HttpClientUtils
  {
    public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public static StringContent Serialize(object o) => new StringContent(JsonConvert.SerializeObject(o, SerializerSettings), Encoding.UTF8, "application/json");

    public static HttpResponseMessage Post(HttpClient httpClient, string route, object request) => httpClient.PostAsync(Constants.ApiRoute + route, HttpClientUtils.Serialize(request)).GetAwaiter().GetResult();
    public static HttpResponseMessage Get(HttpClient httpClient, string route) => httpClient.GetAsync(Constants.ApiRoute + route).GetAwaiter().GetResult();

    public static T GetContent<T>(HttpResponseMessage httpResponseMessage)
    {
      var response = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
      return JsonConvert.DeserializeObject<T>(response, SerializerSettings);
    }
  }
}
