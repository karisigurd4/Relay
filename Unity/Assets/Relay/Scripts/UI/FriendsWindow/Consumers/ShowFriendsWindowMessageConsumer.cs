using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowFriendsWindowMessageConsumer : MessageBusConsumer<ShowFriendsWindowMessage>
  {
    [Header("References")]
    public FriendsWindow FriendsWindowReference;

    public override void OnConsumeMessage(ShowFriendsWindowMessage message)
    {
      if (!FriendsWindowReference.gameObject.activeInHierarchy)
      {
        FriendsWindowReference.gameObject.SetActive(true);
        FriendsWindowReference.RefreshFriendsList();
      }
    }
  }
}