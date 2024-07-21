namespace Relay.DataModel
{
  public class RegisterGameServerConfigurationSPRequest
  {
    public int GameServerId { get; set; }
    public string ServerName { get; set; }
    public bool IsPrivate { get; set; }
    public string Code { get; set; }
    public string Mode { get; set; }
    public int MaxPlayers { get; set; }
  }

  public class RegisterGameServerConfigurationSPResponse
  {
    public bool Success { get; set; }
  }

  public static class RegisterGameServerConfigurationSP
  {
    public static string Name => "[Relay].[RegisterGameServerConfiguration]";

    public static object CreateParameters(int gameServerId, string serverName, bool isPrivate, string code, string mode, int maxPlayers) => new
    {
      gameServerId,
      serverName,
      isPrivate,
      code,
      mode,
      maxPlayers
    };
  }
}
