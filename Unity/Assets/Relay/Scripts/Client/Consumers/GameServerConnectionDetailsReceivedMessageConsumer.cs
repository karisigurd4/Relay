using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerConnectionDetailsReceivedMessageConsumer : MessageBusConsumer<GameServerConnectionDetailsReceivedMessage>
  {
    [Header("References")]
    public GameServerClient GameServerClientReference;

    public override void OnConsumeMessage(GameServerConnectionDetailsReceivedMessage message)
    {
      GameServerClientReference.ConnectToGameServer(message.IPAddress, message.Port);
    }
  }
}
