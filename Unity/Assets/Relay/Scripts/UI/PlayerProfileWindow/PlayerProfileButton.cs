using UnityEngine;

namespace BitterShark.Relay
{
  public class PlayerProfileButton : MonoBehaviour
  {
    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new ShowPlayerProfileWindowMessage()
      {
        PlayerId = RelayPlayerManager.Instance.GetPlayerId()
      });
    }
  }
}