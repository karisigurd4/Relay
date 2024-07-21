using UnityEngine;

namespace BitterShark.Relay
{
  public class HostGameWindowButton : MonoBehaviour
  {
    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new ShowHostGameWindowMessage()
      {

      });
    }
  }
}
