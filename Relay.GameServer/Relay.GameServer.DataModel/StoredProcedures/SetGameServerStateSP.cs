namespace Relay.GameServer.DataModel
{
  public class SetGameServerStateSPRequest
  {
    public int GameServerId { get; set; }
    public string State { get; set; }
  }

  public class SetGameServerStateSPResponse
  {
    public bool Success { get; set; }
  }

  public static class SetGameServerStateSP
  {
    public static string Name => "[Relay].[SetGameServerState]";

    public static object CreateParameters(int gameServerId, string state) => new
    {
      gameServerId,
      state
    };
  }
}
