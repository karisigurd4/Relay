namespace BitterShark.Relay
{
  public class PlayerRegisteredMessage
  {
    public string PlayerName { get; set; }
    public int PlayerId { get; set; }
    public string PlayerApiKey { get; set; }
  }
}