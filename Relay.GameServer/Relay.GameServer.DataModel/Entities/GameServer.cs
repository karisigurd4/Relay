using Relay.GameServer.Core.Types;
using System;

namespace Relay.GameServer.DataModel
{
  public class GameServer
  {
    public int Id { get; set; }
    public int ProcessId { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public DateTime StartDate { get; set; }
    public GameServerState State { get; set; }
  }
}
