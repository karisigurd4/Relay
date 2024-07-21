namespace BitterShark.Relay
{
  public class ClientDisconnectedMessageConsumer : MessageBusConsumer<ClientDisconnectedMessage>
  {
    public override void OnConsumeMessage(ClientDisconnectedMessage message)
    {
      GameStateRepository.HandleClientDisconnected(message.ClientId);
    }
  }
}
