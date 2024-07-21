using Relay.Contracts;
using System;
using UnityEngine;

namespace BitterShark.Relay
{
  public class NotificationMessageApiClient : MonoBehaviour
  {
    private static NotificationMessageApiClient _instance;
    public static NotificationMessageApiClient Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;
    }

    public void GetUnreadNotificationsCount(string playerApiKey, Action<GetUnreadNotificationMessagesCountResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.NotificationMessageApiRoute + "/count", new GetUnreadNotificationMessagesCountRequest()
      {
        PlayerApiKey = playerApiKey
      }, responseCallback));
    }

    public void GetNotificationMessages(string playerApiKey, int count, int offset, Action<GetNotificationMessagesResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.NotificationMessageApiRoute + "/", new GetNotificationMessagesRequest()
      {
        ApiKey = playerApiKey,
        Count = count,
        Offset = offset
      }, responseCallback));
    }

    public void AnswerNotification(string projectId, string playerApiKey, int notificationId, Answer answer, Action<AnswerNotificationResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.NotificationMessageApiRoute + "/answer", new AnswerNotificationRequest()
      {
        ProjectId = projectId,
        Answer = answer,
        NotificationMessageId = notificationId,
        PlayerApiKey = playerApiKey
      }, responseCallback));
    }
  }
}
