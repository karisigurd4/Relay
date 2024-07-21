namespace Relay.Contracts
{
  using System;

  public class GameServer
  {
    public int Id { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public DateTime StartDate { get; set; }
  }
}
