using Newtonsoft.Json;
using System;

namespace Relay.Core.Contracts
{
  public class AccessToken
  {
    [JsonProperty("access_token")]
    public string Token { get; set; }
    [JsonProperty("expires_in")]
    public DateTime ExpirationDate { get; set; }
    [JsonProperty("token_type")]
    public string Type { get; set; }
  }
}
