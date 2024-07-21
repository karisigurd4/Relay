using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class NotificationsBell : MonoBehaviour
  {
    [Header("References")]
    public GameObject UnreadCountContainer;
    public TextMeshProUGUI UnreadCountText;

    private static float refreshRate = .5f;
    private float lastRefreshTime;

    void Start()
    {
      lastRefreshTime = Time.time;
    }

    void Update()
    {
      if (!RelayPlayerManager.Instance.IsPlayerRegistered())
      {
        return;
      }

      if (Time.time > lastRefreshTime + refreshRate)
      {
        lastRefreshTime = Time.time;

        if (NotificationMessageApiClient.Instance != null)
        {
          NotificationMessageApiClient.Instance.GetUnreadNotificationsCount(RelayPlayerManager.Instance.GetPlayerApiKey(), response =>
          {
            if (response.Count > 0)
            {
              UnreadCountText.text = response.Count.ToString();

              if (!UnreadCountContainer.activeInHierarchy || UnreadCountText.text != response.Count.ToString())
              {
                MessageBusManager.Instance.Publish(new NotificationReceivedMessage()
                {
                });
              }

              UnreadCountContainer.SetActive(true);
            }
            else
            {
              UnreadCountContainer.SetActive(false);
            }
          });
        }
      }
    }
  }
}
