using System;

namespace Relay.DataModel
{
  public class PlayerStatistics
  {
    public Guid ProjectId { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int PlayerCount { get; set; }
  }
}
