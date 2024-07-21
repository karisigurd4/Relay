using UnityEngine;

namespace BitterShark.Relay
{
  [RequireComponent(typeof(TransitionOutGameServerStateUpdatedMessageConsumer))]
  public class TransitionOutTransitionFinishedMessageConsumer : MessageBusConsumer<TransitionFinishedMessage>
  {
    [Header("Scene configurations")]
    public RelaySceneConfiguration PostGameSceneConfiguration;

    public override void OnConsumeMessage(TransitionFinishedMessage message)
    {
      if (GetComponent<TransitionOutGameServerStateUpdatedMessageConsumer>().Triggered)
      {
        RelaySceneManager.Instance.LoadSceneConfiguration(PostGameSceneConfiguration);
      }
    }
  }
}
