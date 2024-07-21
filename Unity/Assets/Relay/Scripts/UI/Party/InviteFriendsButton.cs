using UnityEngine;

namespace BitterShark.Relay
{
  public class InviteFriendsButton : MonoBehaviour
  {
    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new ShowFriendsWindowMessage() { });
    }
  }
}