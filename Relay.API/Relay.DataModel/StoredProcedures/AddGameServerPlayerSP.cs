namespace Relay.DataModel
{
  public class AddGameServerPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class AddGameServerPlayerSPRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
    public string GameServerHost { get; set; }
  }

  public static class AddGameServerPlayerSP
  {
    public static string Name => "[Relay].[AddGameServerPlayer]";

    public static object CreateParameters(string gameServerHost, int gameServerId, int playerId) => new
    {
      gameServerHost,
      gameServerId,
      playerId
    };
  }
}
