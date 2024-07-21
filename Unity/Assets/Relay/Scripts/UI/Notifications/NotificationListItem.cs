using Relay.Contracts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class NotificationListItem : MonoBehaviour
  {
    [Header("References")]
    public Image NotificationIconImage;
    public TextMeshProUGUI NotificationDataText;
    public TextMeshProUGUI TimeSinceText;

    [Space(10)]
    [Header("Configuration")]
    public Sprite InformationIconSprite;
    public Sprite FriendRequestIconSprite;
    public Sprite PartyInvitationIconSprite;

    private NotificationType notificationType;
    private int notificationId;

    public void Initialize(int notificationId, NotificationType notificationType, string notificationData, DateTime notificationTime)
    {
      this.notificationType = notificationType;
      this.notificationId = notificationId;

      switch (notificationType)
      {
        case NotificationType.Info: NotificationIconImage.sprite = InformationIconSprite; break;
        case NotificationType.FriendRequest: NotificationIconImage.sprite = FriendRequestIconSprite; break;
        case NotificationType.PartyInvite: NotificationIconImage.sprite = PartyInvitationIconSprite; break;
      }

      NotificationDataText.text = notificationData;

      var timeDifference = DateTime.UtcNow - notificationTime;

      if (timeDifference.Hours < 25)
      {
        TimeSinceText.text = $"{timeDifference.Hours} hours ago";
      }
      else
      {
        TimeSinceText.text = $"{timeDifference.Days} days ago";
      }
    }

    public void OnClick()
    {
      if (notificationType == NotificationType.Info)
      {
        MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
        {
          InformationData = NotificationDataText.text,
          LeftHeaderText = notificationType.ToString(),
          RightHeaderText = TimeSinceText.text
        });
      }
      else if (notificationType == NotificationType.FriendRequest || notificationType == NotificationType.PartyInvite)
      {
        MessageBusManager.Instance.Publish(new ShowConfirmationDialogMessage()
        {
          Text = $"{NotificationDataText.text}",
          OnConfirmAction = () =>
          {
            NotificationMessageApiClient.Instance.AnswerNotification(RelayProjectSettings.GetProjectSettings().ProjectId, RelayPlayerManager.Instance.GetPlayerApiKey(), notificationId, Answer.Yes, response =>
            {
              if (notificationType == NotificationType.FriendRequest)
              {
                MessageBusManager.Instance.Publish(new FriendListUpdatedMessage() { });
              }
            });
          }
        });
      }
    }
  }
}
