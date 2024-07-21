using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowChangeNameWindowMessageConsumer : MessageBusConsumer<ShowChangeNameWindowMessage>
  {
    [Header("References")]
    public GameObject Window;

    public override void OnConsumeMessage(ShowChangeNameWindowMessage message)
    {
      Window.SetActive(true);
    }
  }
}