using UnityEngine;

namespace BitterShark.Relay
{
  public class FriendListUpdatedMessageConsumer : MessageBusConsumer<FriendListUpdatedMessage>
  {
    [Header("References")]
    public FriendsWindow FriendsWindow;

    public override void OnConsumeMessage(FriendListUpdatedMessage message)
    {
      FriendsWindow.RefreshFriendsList();
    }
  }
}