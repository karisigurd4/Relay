using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowHostGameWindowMessageConsumer : MessageBusConsumer<ShowHostGameWindowMessage>
  {
    [Header("References")]
    public GameObject HostGameWindow;

    public override void OnConsumeMessage(ShowHostGameWindowMessage message)
    {
      HostGameWindow.SetActive(true);
    }
  }
}