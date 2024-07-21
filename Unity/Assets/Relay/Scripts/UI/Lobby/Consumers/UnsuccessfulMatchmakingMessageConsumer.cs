using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class UnsuccessfulMatchmakingMessageConsumer : MessageBusConsumer<UnsuccessfulMatchmakingMessage>
  {
    [Header("References")]
    public TextMeshProUGUI ConnectingText;
    public TextMeshProUGUI StatusText;

    public override void OnConsumeMessage(UnsuccessfulMatchmakingMessage message)
    {
      ConnectingText.text = "Unable to connect";
      StatusText.text = message.Message;
    }
  }
}