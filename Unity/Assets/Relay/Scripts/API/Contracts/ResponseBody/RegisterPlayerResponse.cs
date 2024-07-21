namespace Relay.Contracts
{
  public class RegisterPlayerResponse : BaseResponse
  {
    public string ApiKey { get; set; }
    public int Id { get; set; }
  }
}
