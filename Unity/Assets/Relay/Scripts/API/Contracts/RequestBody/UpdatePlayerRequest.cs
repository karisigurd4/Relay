namespace Relay.Contracts
{
  public class UpdatePlayerRequest
  {
    public string ApiKey { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public bool Public { get; set; }
  }
}
