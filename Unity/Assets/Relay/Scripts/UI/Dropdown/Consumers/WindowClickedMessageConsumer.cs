using UnityEngine;

namespace BitterShark.Relay
{
  public class WindowClickedMessageConsumer : MessageBusConsumer<WindowClickedMessage>
  {
    public GameObject Dropdown;

    public override void OnConsumeMessage(WindowClickedMessage message)
    {
      Dropdown.SetActive(false);
    }
  }
}
