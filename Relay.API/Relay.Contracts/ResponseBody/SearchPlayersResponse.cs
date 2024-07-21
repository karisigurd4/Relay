namespace Relay.Contracts
{
  public class SearchPlayersResponse : BaseResponse
  {
    public int TotalMatches { get; set; }
    public Player[] Players { get; set; }
  }
}
