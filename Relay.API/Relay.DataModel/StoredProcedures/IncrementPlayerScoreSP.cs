namespace Relay.DataModel
{
  public class IncrementPlayerScoreSPRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
    public string ScoreType { get; set; }
    public int Amount { get; set; }
  }

  public class IncrementPlayerScoreSPResponse
  {
    public bool Success { get; set; }
  }



  public static class IncrementPlayerScoreSP
  {
    public static string Name => "[Relay].[IncrementPlayerScore]";

    public static object CreateParameters(int gameServerId, int playerId, int hashedScoreType, string scoreType, int amount) => new
    {
      gameServerId,
      playerId,
      hashedScoreType,
      scoreType,
      amount
    };
  }
}