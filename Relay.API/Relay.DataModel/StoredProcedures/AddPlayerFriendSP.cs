namespace Relay.DataModel
{
  public class AddPlayerFriendSPResponse
  {
    public bool Success { get; set; }
  }

  public class AddPlayerFriendSPRequest
  {
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }
  }

  public static class AddPlayerFriendSP
  {
    public static string Name => "[Relay].[AddPlayerFriend]";

    public static object CreateParameters(int player1Id, int player2Id) => new
    {
      player1Id,
      player2Id
    };
  }
}
