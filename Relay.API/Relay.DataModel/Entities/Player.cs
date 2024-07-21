namespace Relay.DataModel
{
  using System;

  public class Player
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public Guid ApiKey { get; set; }
    public DateTime RegistrationDate { get; set; }
    public PlayerData[] PublicPlayerData { get; set; }
    public PlayerData[] PrivatePlayerData { get; set; }
    public bool PlayerActive { get; set; }
  }
}
