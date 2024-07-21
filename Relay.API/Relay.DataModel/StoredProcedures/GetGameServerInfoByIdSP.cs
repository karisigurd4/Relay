namespace Relay.DataModel
{
  using System;

  public class GetGameServerInfoByIdSPResponseJson
  {
    public bool Success { get; set; }
    public int Id { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StopDate { get; set; }
    public string State { get; set; }
    public string PlayersJson { get; set; }
    public int PlayerCount { get; set; }
  }

  public class GetGameServerInfoByIdSPResponse
  {
    public bool Success { get; set; }
    public int Id { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StopDate { get; set; }
    public string State { get; set; }
    public GameServerPlayerInfo[] Players { get; set; }
    public int PlayerCount { get; set; }
  }

  public class GetGameServerInfoByIdSPRequest
  {
    public int GameServerId { get; set; }
  }

  public static class GetGameServerInfoByIdSP
  {
    public static string Name => "[Relay].[GetGameServerInfoById]";

    public static object CreateParameters(int gameServerId) => new
    {
      gameServerId = gameServerId
    };
  }
}
