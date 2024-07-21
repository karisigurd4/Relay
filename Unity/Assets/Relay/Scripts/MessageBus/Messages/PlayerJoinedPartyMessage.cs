using UnityEngine;

namespace BitterShark.Relay
{
  public class PlayerJoinedPartyMessage
  {
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public Sprite PlayerProfilePicture { get; set; }
  }
}