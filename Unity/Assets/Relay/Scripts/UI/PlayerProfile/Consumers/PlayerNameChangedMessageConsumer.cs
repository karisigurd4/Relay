using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class PlayerNameChangedMessageConsumer : MessageBusConsumer<PlayerNameChangedMessage>
  {
    [Header("References")]
    public TextMeshProUGUI PlayerNameText;

    public override void OnConsumeMessage(PlayerNameChangedMessage message)
    {
      PlayerNameText.text = message.NewPlayerName;
    }
  }
}