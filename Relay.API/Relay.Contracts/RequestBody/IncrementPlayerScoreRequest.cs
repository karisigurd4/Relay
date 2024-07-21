namespace Relay.Contracts
{
  public class IncrementPlayerScoreRequest
  {
    public int GameServerId { get; set; }
    public int PlayerId { get; set; }
    public string ScoreType { get; set; }
    public int Amount { get; set; }
  }
}
