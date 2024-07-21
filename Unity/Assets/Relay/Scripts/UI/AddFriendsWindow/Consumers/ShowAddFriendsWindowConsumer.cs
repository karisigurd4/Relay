using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowAddFriendsWindowConsumer : MessageBusConsumer<ShowAddFriendsWindowMessage>
  {
    [Header("Reference")]
    public AddFriendsWindow AddFriendsWindowReference;

    public override void OnConsumeMessage(ShowAddFriendsWindowMessage message)
    {
      AddFriendsWindowReference.gameObject.SetActive(true);
    }
  }
}