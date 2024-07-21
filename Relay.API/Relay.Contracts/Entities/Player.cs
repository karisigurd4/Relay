namespace Relay.Contracts
{
  using System;
  using System.Collections.Generic;

  public class Player
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool PlayerActive { get; set; }
    public Dictionary<string, string> Data_Private { get; set; }
    public Dictionary<string, string> Data_Public { get; set; }
  }
}
