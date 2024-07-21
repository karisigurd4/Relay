using UnityEngine;

namespace BitterShark.Relay
{
  public class UpdateTransitionOverlayConsumer : MessageBusConsumer<UpdateTransitionOverlayMessage>
  {
    [Header("References")]
    public TransitionOverlay TransitionOverlayReference;

    public override void OnConsumeMessage(UpdateTransitionOverlayMessage message)
    {
      TransitionOverlayReference.TransitionIn = message.On;
    }
  }
}