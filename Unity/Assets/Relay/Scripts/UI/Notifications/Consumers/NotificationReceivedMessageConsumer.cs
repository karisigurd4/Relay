using UnityEngine;

namespace BitterShark.Relay
{
  public class NotificationReceivedMessageConsumer : MessageBusConsumer<NotificationReceivedMessage>
  {
    [Header("References")]
    public NotificationList NotificationListReference;

    public override void OnConsumeMessage(NotificationReceivedMessage message)
    {
      if (NotificationListReference.NotificationsListContainerReference.activeInHierarchy)
      {
        NotificationListReference.RefreshNotifications();
      }
    }
  }
}