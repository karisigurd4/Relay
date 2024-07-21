namespace Relay.GameServer.DataModel
{
  public class StopGameServerSPRequest
  {
    public int GameServerId { get; set; }
  }

  public class StopGameServerSPResponse
  {

  }

  public static class StopGameServerSP
  {
    public static string Name => "[Relay].[StopGameServer]";

    public static object CreateParameters(int gameServerId) => new
    {
      gameServerId
    };
  }
}
