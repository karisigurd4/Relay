using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class InGameDebugGameServerConnectionDetailsReceivedMessageConsumer : MessageBusConsumer<GameServerConnectionDetailsReceivedMessage>
  {
    [Header("References")]
    public TextMeshProUGUI GameServerIdValueText;

    public override void OnConsumeMessage(GameServerConnectionDetailsReceivedMessage message)
    {
      GameServerIdValueText.text = message.GameServerId.ToString();
    }
  }
}