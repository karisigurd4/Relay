using System;

namespace Relay.Contracts
{
  public class GameServerStatistics
  {
    public Guid ProjectId { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int GameServersCount { get; set; }
  }
}
