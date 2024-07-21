using UnityEngine;

namespace BitterShark.Relay
{
  public class Window : MonoBehaviour
  {
    public void OnEnable()
    {
      BringToTop();
    }

    public void OnClick()
    {
      BringToTop();

      MessageBusManager.Instance.Publish(new WindowClickedMessage() { });
    }

    public void BringToTop()
    {
      transform.parent.SetAsLastSibling();
    }
  }
}