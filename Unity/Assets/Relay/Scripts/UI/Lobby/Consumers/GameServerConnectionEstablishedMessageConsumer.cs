using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerConnectionEstablishedMessageConsumer : MessageBusConsumer<GameServerConnectionEstablishedMessage>
  {
    [Header("References")]
    public GameObject LobbyUI;
    public GameObject ConnectionUI;

    public override void OnConsumeMessage(GameServerConnectionEstablishedMessage message)
    {
      CoroutineUtility.Instance.StartCoroutine(0.5f, () =>
      {
        if (LobbyUI != null)
        {
          LobbyUI?.gameObject.SetActive(true);
        }

        if (ConnectionUI?.gameObject != null)
        {
          ConnectionUI.SetActive(false);
        }
      });
    }
  }
}