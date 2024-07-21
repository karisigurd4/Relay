using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerConnectionDetailsReceivedMessageConsumer : MessageBusConsumer<GameServerConnectionDetailsReceivedMessage>
  {
    [Header("References")]
    public TextMeshProUGUI ConnectionStatusTextReference;

    public override void OnConsumeMessage(GameServerConnectionDetailsReceivedMessage message)
    {
      if (LobbyUIWatcher.Instance != null)
      {
        LobbyUIWatcher.Instance.SetGameServerId(message.GameServerId);
      }

      ConnectionStatusTextReference.text = "connecting to game server";
    }
  }
}