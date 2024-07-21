namespace Relay.GameServer.DataModel
{
  public class SetGameServerProcessIdSPResponse
  {
    public bool Success { get; set; }
  }

  public class SetGameServerProcessIdSPRequest
  {
    public int GameServerId { get; set; }
    public int ProcessId { get; set; }
  }

  public static class SetGameServerProcessIdSP
  {
    public static string Name => "[Relay].[SetGameServerProcessId]";

    public static object CreateParameters(int gameServerId, int processId) => new
    {
      gameServerId,
      processId
    };
  }
}
