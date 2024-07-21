using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowJoinWithCodeWindowMessageConsumer : MessageBusConsumer<ShowJoinWithCodeWindowMessage>
  {
    [Header("References")]
    public Window JoinWithCodeWindow;

    public override void OnConsumeMessage(ShowJoinWithCodeWindowMessage message)
    {
      JoinWithCodeWindow.gameObject.SetActive(true);
      JoinWithCodeWindow.BringToTop();
    }
  }
}
