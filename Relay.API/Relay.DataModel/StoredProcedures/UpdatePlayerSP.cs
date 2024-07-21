namespace Relay.DataModel
{
  using System;

  public class UpdatePlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class UpdatePlayerSPRequest
  {
    public Guid ApiKey { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool Public { get; set; }
  }

  public static class UpdatePlayerSP
  {
    public static string Name => "[Relay].[UpdatePlayer]";

    public static object CreateParameters(Guid playerApiKey, string key, string value, bool publicData) => new
    {
      playerApiKey,
      key,
      value,
      publicData
    };
  }
}
