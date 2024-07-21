namespace Relay.GameServer.DataModel
{
  public class AddGameServerPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class AddGameServerPlayerSPRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
  }

  public static class AddGameServerPlayerSP
  {
    public static string Name => "[Relay].[AddGameServerPlayer]";

    public static object CreateParameters(int gameServerId, int playerId) => new
    {
      gameServerId,
      playerId
    };
  }
}
