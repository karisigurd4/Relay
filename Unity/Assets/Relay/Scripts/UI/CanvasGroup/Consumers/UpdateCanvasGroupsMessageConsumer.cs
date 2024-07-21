using UnityEngine;

namespace BitterShark.Relay
{
  public class UpdateCanvasGroupsMessageConsumer : MessageBusConsumer<UpdateCanvasGroupsMessage>
  {
    [Header("References")]
    public CanvasGroupManager CanvasGroupManagerReference;

    public override void OnConsumeMessage(UpdateCanvasGroupsMessage message)
    {
      CanvasGroupManagerReference.On = message.On;
    }
  }
}