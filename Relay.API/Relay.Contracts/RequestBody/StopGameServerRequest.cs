namespace Relay.Contracts
{
  using System;

  public class StopGameServerRequest
  {
    public int GameServerId { get; set; }
    public Guid ApiKey { get; set; }
  }
}
