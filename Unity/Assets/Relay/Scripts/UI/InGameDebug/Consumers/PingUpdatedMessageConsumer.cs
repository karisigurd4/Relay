using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class PingUpdatedMessageConsumer : MessageBusConsumer<PingUpdatedMessage>
  {
    [Header("References")]
    public TextMeshProUGUI PingValueText;

    public override void OnConsumeMessage(PingUpdatedMessage message)
    {
      PingValueText.text = message.Ping.ToString();
    }
  }
}