namespace Relay.Contracts
{
  public class GetPlayerStatisticsRequest
  {
    public string ProjectId { get; set; }
    public string ExtAuthId { get; set; }
    public Period Period { get; set; }
  }
}
