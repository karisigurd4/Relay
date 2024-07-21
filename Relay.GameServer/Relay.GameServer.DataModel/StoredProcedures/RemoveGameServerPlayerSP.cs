namespace Relay.GameServer.DataModel
{
  public class RemoveGameServerPlayerSPResponse
  {
    public bool Success { get; set; }
  }

  public class RemoveGameServerPlayerSPRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
  }

  public static class RemoveGameServerPlayerSP
  {
    public static string Name => "[Relay].[RemoveGameServerPlayer]";

    public static object CreateParameters(int gameServerId, int playerId) => new
    {
      gameServerId,
      playerId
    };
  }
}
