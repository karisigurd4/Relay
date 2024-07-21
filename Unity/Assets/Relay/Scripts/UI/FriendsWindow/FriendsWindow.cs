using System.Linq;
using UnityEngine;

namespace BitterShark.Relay
{
  public class FriendsWindow : MonoBehaviour
  {
    [Header("Prefabs")]
    public PlayerProfileCard PlayerProfileCardPrefab;
    public GameObject OnlineFriendsListHeaderPrefab;
    public GameObject OfflineFriendsListHeaderPrefab;

    [Space(10)]
    [Header("References")]
    public Transform FriendsList;
    public LoaderUI LoaderUI;

    public void RefreshFriendsList()
    {
      LoaderUI.StartLoading();

      PlayerApiClient.Instance.GetPlayerFriendsList(RelayPlayerManager.Instance.GetPlayerApiKey(), response =>
      {
        LoaderUI.StopLoading();

        if (response == null)
        {
          Debug.LogError($"Received null response when refreshing friends list. Check your project's API key under Window/Relay/Startup.");
        }
        else
        {
          foreach (Transform t in FriendsList.transform)
          {
            Destroy(t.gameObject);
          }

          var onlineFriends = response.Players.Where(x => x.PlayerActive).ToArray();
          var offlineFriends = response.Players.Except(onlineFriends).ToArray();

          if (onlineFriends.Length > 0)
          {
            var header = Instantiate(OnlineFriendsListHeaderPrefab, FriendsList.transform);

            for (int i = 0; i < onlineFriends.Length; i++)
            {
              var playerCard = Instantiate(PlayerProfileCardPrefab, FriendsList.transform);
              playerCard.Initialize(onlineFriends[i].Id, onlineFriends[i].Name, onlineFriends[i].PlayerActive, null);
            }
          }

          if (offlineFriends.Length > 0)
          {
            var header = Instantiate(OfflineFriendsListHeaderPrefab, FriendsList.transform);

            for (int i = 0; i < offlineFriends.Length; i++)
            {
              var playerCard = Instantiate(PlayerProfileCardPrefab, FriendsList.transform);
              playerCard.Initialize(offlineFriends[i].Id, offlineFriends[i].Name, offlineFriends[i].PlayerActive, null);
            }
          }
        }
      });
    }

    public void OnClickAddFriends()
    {
      MessageBusManager.Instance.Publish(new ShowAddFriendsWindowMessage() { });
      gameObject.SetActive(false);
    }
  }

}
