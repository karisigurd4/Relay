using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class AddFriendsWindow : MonoBehaviour
  {
    [Header("References")]
    public GameObject ProfileSearchResultsReference;
    public TextMeshProUGUI SearchBarInputTextReference;
    public TextMeshProUGUI TotalMatchesTextReference;
    public LoaderUI LoaderUI;

    [Space(10)]
    [Header("Prefabs")]
    public PlayerProfileSearchResult PlayerProfileSearchResultPrefab;

    private bool searching = false;

    public void SearchPlayers()
    {
      if (searching)
      {
        return;
      }

      LoaderUI.StartLoading();
      searching = true;

      PlayerApiClient.Instance.SearchPlayers(RelayProjectSettings.GetProjectSettings().ProjectId, SearchBarInputTextReference.text, response =>
      {
        foreach (Transform t in ProfileSearchResultsReference.transform)
        {
          Destroy(t.gameObject);
        }

        if (response == null)
        {
          MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
          {
            InformationData = "Could not fetch player data",
            LeftHeaderText = "Service error",
            RightHeaderText = ""
          });
          Debug.LogError($"Received null response when refreshing notifications. Check your project's API key under Window/Relay/Startup.");
          return;
        }

        CoroutineUtility.Instance.StartCoroutine(0.5f, () =>
        {
          LoaderUI.StopLoading();
          searching = false;

          if (!TotalMatchesTextReference.gameObject.activeInHierarchy)
          {
            TotalMatchesTextReference.gameObject.SetActive(true);
          }

          TotalMatchesTextReference.text = $"Total: {response.TotalMatches}";

          if (response.Players.Length > 0)
          {
            for (int i = 0; i < response.Players.Length; i++)
            {
              if (response.Players[i].Id == RelayPlayerManager.PlayerId)
              {
                continue;
              }

              var profile = Instantiate(PlayerProfileSearchResultPrefab, ProfileSearchResultsReference.transform);
              profile.Initialize(null, response.Players[i].Name, response.Players[i].Id);
            }
          }
        });
      });
    }
  }
}