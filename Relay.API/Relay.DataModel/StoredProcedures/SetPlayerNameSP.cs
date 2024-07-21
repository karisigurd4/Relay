namespace Relay.DataModel
{
  using System;

  public class SetPlayerNameSPRequest
  {
    public string PlayerApiKey { get; set; }
    public string NewPlayerName { get; set; }
  }

  public class SetPlayerNameSPResponse
  {
    public bool Success { get; set; }
  }

  public static class SetPlayerNameSP
  {
    public static string Name => "[Relay].[SetPlayerName]";

    public static object CreateParameters(Guid playerApiKey, string newPlayerName) => new
    {
      playerApiKey,
      newPlayerName
    };
  }
}
