using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowFindGameServerWindowMessageConsumer : MessageBusConsumer<ShowFindGameServerWindowMessage>
  {
    [Header("References")]
    public FindingGameServerWindow FindingGameServerWindowReference;

    public override void OnConsumeMessage(ShowFindGameServerWindowMessage message)
    {
      Debug.Log($"Received!");
      FindingGameServerWindowReference.gameObject.SetActive(true);
    }
  }
}