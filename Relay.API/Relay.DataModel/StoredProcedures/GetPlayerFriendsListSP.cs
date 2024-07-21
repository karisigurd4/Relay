using System;

namespace Relay.DataModel
{
  public class GetPlayerFriendsListSPRequest
  {
    public Guid ApiKey { get; set; }
  }

  public class GetPlayerFriendsListSPResponseJson
  {
    public bool Success { get; set; }
    public string PlayersJson { get; set; }
  }

  public class GetPlayerFriendsListSPResponse
  {
    public bool Success { get; set; }
    public Player[] Players { get; set; }
  }

  public class GetPlayerFriendsListSP
  {
    public static string Name => "[Relay].[GetPlayerFriendsList]";

    public static object CreateParameters(Guid apiKey) => new
    {
      apiKey
    };
  }
}
