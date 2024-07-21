namespace BitterShark.Relay
{
  public class GameServerConnectionDetailsReceivedMessageConsumer : MessageBusConsumer<GameServerConnectionDetailsReceivedMessage>
  {
    public override void OnConsumeMessage(GameServerConnectionDetailsReceivedMessage message)
    {
      if (RelayPlayerManager.Instance != null)
      {
        RelayPlayerManager.Instance.CurrentGameServerId = message.GameServerId;
      }
    }
  }
}