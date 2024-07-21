namespace Relay.DataModel
{
  public class GetGameServerByIdSPRequest
  {
    public int GameServerId { get; set; }
  }

  public class GetGameServerByIdSPResponseJson
  {
    public bool Success { get; set; }
    public string GameServerJson { get; set; }
  }

  public static class GetGameServerByIdSP
  {
    public static string Name => "[Relay].[GetGameServerById]";

    public static object CreateParameters(int gameServerId) => new
    {
      gameServerId
    };
  }
}
