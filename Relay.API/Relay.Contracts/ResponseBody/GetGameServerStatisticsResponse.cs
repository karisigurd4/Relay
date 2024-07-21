namespace Relay.Contracts
{
  public class GetGameServerStatisticsResponse : BaseResponse
  {
    public GameServerStatistics[] GameServerStatistics { get; set; }
  }
}
