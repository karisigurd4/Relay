using UnityEngine;

namespace BitterShark.Relay
{
  public class FriendsButton : MonoBehaviour
  {
    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new ShowFriendsWindowMessage()
      {
      });
    }
  }
}