using Relay.Toolkit.Networking;
using UnityEngine;

namespace BitterShark.Relay
{
  public class TransitionOutGameServerStateUpdatedMessageConsumer : MessageBusConsumer<GameServerStateUpdatedMessage>
  {
    [Header("Scene configurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("State")]
    public bool Triggered = false;

    public override void OnConsumeMessage(GameServerStateUpdatedMessage message)
    {
      if (message.GameServerState == GameServerState.Finished)
      {
        Triggered = true;
        Debug.Log("Loaded");
        RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
      }
    }
  }
}
