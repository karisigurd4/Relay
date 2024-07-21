using TMPro;

namespace BitterShark.Relay
{
  public class PlayerRegisteredMessageConsumer : MessageBusConsumer<PlayerRegisteredMessage>
  {
    public TextMeshProUGUI PlayerNameText;

    public override void OnConsumeMessage(PlayerRegisteredMessage message)
    {
      PlayerNameText.text = message.PlayerName;
    }
  }
}