namespace BitterShark.Relay
{
  public class TransitionOutFinishedMessageConsumer : MessageBusConsumer<TransitionFinishedMessage>
  {
    public override void OnConsumeMessage(TransitionFinishedMessage message)
    {
      if (RelaySceneManager.Instance != null)
      {
        RelaySceneManager.Instance.UnloadSceneByName("Assets/Relay/Scenes/TransitionOut.unity");
      }
    }
  }
}