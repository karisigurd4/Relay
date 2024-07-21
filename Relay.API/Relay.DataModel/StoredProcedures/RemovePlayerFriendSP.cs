namespace Relay.DataModel
{
  using System;

  public class RemovePlayerFriendSPRequest
  {
    public Guid ApiKey { get; set; }
    public int RemoveFriendPlayerId { get; set; }
  }

  public class RemovePlayerFriendSPResponse
  {
    public bool Success { get; set; }
  }

  public static class RemovePlayerFriendSP
  {
    public static string Name => "[Relay].[RemovePlayerFriend]";

    public static object CreateParameters(Guid apiKey, int removePlayerId) => new
    {
      apiKey,
      removePlayerId
    };
  }
}
