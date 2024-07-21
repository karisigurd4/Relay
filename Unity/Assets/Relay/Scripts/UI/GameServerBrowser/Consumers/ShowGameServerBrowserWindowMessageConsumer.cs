using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowGameServerBrowserWindowMessageConsumer : MessageBusConsumer<ShowGameServerBrowserWindowMessage>
  {
    [Header("References")]
    public GameObject GameServerBrowserWindow;

    public override void OnConsumeMessage(ShowGameServerBrowserWindowMessage message)
    {
      GameServerBrowserWindow.SetActive(true);
    }
  }
}
