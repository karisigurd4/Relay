using System;

namespace Relay.Contracts
{
  public class GetGameServerInfoByIdResponse : BaseResponse
  {
    public int Id { get; set; }
    public string IPAddress { get; set; }
    public int Port { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime StopDate { get; set; }
    public string State { get; set; }
    public GameServerPlayerInfo[] Players { get; set; }
    public int PlayerCount { get; set; }
  }
}
