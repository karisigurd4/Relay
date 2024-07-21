using System;
using System.Linq;
using UnityEngine;

namespace BitterShark.Relay
{
  public class NotificationList : MonoBehaviour
  {
    [Header("References")]
    public GameObject NotificationsListContainerReference;
    public GameObject NotificationsListContentReferemce;
    public LoaderUI LoaderUI;

    [Space(10)]
    [Header("Prefabs")]
    public NotificationListItem NotificationListItemPrefab;
    public GameObject UnreadNotificationsTitlePrefab;
    public GameObject SeenNotificationsTitlePrefab;

    public void RefreshNotifications()
    {
      LoaderUI.StartLoading();

      NotificationMessageApiClient.Instance.GetNotificationMessages(RelayPlayerManager.Instance.GetPlayerApiKey(), 30, 0, response =>
      {
        LoaderUI.StopLoading();

        if (response == null)
        {
          Debug.LogError($"Received null response when refreshing notifications. Check your project's API key under Window/Relay/Startup.");
        }
        else
        {
          foreach (Transform t in NotificationsListContentReferemce.transform)
          {
            Destroy(t.gameObject);
          }

          var unreadNotifications = response.NotificationMessages.Where(x => !x.ViewedFlag).OrderByDescending(x => x.SentDateTime).ToArray();
          var readNotifications = response.NotificationMessages.Except(unreadNotifications).OrderByDescending(x => x.SentDateTime).ToArray();

          if (unreadNotifications.Length > 0)
          {
            var unreadTitle = Instantiate(UnreadNotificationsTitlePrefab, NotificationsListContentReferemce.transform);

            for (int i = 0; i < unreadNotifications.Length; i++)
            {
              var notificationListItem = Instantiate(NotificationListItemPrefab, NotificationsListContentReferemce.transform);
              notificationListItem.Initialize(readNotifications[i].Id, (NotificationType)Enum.Parse(typeof(NotificationType), unreadNotifications[i].Type), unreadNotifications[i].Data, unreadNotifications[i].SentDateTime);
            }
          }

          if (readNotifications.Length > 0)
          {
            var readTitle = Instantiate(SeenNotificationsTitlePrefab, NotificationsListContentReferemce.transform);

            for (int i = 0; i < readNotifications.Length; i++)
            {
              var notificationListItem = Instantiate(NotificationListItemPrefab, NotificationsListContentReferemce.transform);
              notificationListItem.Initialize(readNotifications[i].Id, (NotificationType)Enum.Parse(typeof(NotificationType), readNotifications[i].Type), readNotifications[i].Data, readNotifications[i].SentDateTime);
            }
          }
        }
      });
    }

    public void Toggle()
    {
      NotificationsListContainerReference.SetActive(!NotificationsListContainerReference.activeInHierarchy);

      if (NotificationsListContainerReference.activeInHierarchy)
      {
        RefreshNotifications();
      }
    }
  }
}
