using UnityEngine;

namespace BitterShark.Relay
{
  public class Background : MonoBehaviour
  {
    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new BackgroundClickedMessage());
    }
  }
}