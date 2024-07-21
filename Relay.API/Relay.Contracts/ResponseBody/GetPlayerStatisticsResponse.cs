namespace Relay.Contracts
{
  public class GetPlayerStatisticsResponse : BaseResponse
  {
    public PlayerStatistics[] PlayerStatistics { get; set; }
  }
}
