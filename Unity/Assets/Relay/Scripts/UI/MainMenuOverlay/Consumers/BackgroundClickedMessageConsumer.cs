using UnityEngine;

namespace BitterShark.Relay
{
  public class BackgroundClickedMessageConsumer : MessageBusConsumer<BackgroundClickedMessage>
  {
    [Header("Window references")]
    public GameObject[] Windows;

    public override void OnConsumeMessage(BackgroundClickedMessage message)
    {
      for (int i = 0; i < Windows.Length; i++)
      {
        if (Windows[i].activeInHierarchy)
        {
          Windows[i].SetActive(false);
        }
      }
    }
  }
}