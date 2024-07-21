using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowPlayerProfileWindowMessageConsumer : MessageBusConsumer<ShowPlayerProfileWindowMessage>
  {
    [Header("References")]
    public PlayerProfileWindow PlayerProfileWindow;

    public override void OnConsumeMessage(ShowPlayerProfileWindowMessage message)
    {
      PlayerProfileWindow.gameObject.SetActive(true);
      PlayerProfileWindow.Show(message.PlayerId);
    }
  }
}