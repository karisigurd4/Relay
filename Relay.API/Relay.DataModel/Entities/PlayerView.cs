using System;
using System.Collections.Generic;

namespace Relay.DataModel
{
  public class PlayerViewJson
  {
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string PublicPlayerDataJson { get; set; }
    public string PrivatePlayerDataJson { get; set; }
    public bool PlayerActive { get; set; }
    public bool InGameServer { get; set; }
    public int InGameServerId { get; set; }
    public string InGameServerIPAddress { get; set; }
    public int InGameServerPort { get; set; }
  }

  public class PlayerView
  {
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public Dictionary<string, string> PublicPlayerData { get; set; }
    public Dictionary<string, string> PrivatePlayerData { get; set; }
    public bool PlayerActive { get; set; }
    public bool InGameServer { get; set; }
    public int InGameServerId { get; set; }
    public string InGameServerIPAddress { get; set; }
    public int InGameServerPort { get; set; }
  }
}
