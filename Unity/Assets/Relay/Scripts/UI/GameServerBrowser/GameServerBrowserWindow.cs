using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class GameServerBrowserWindow : MonoBehaviour
  {
    [Header("Prefabs")]
    public ServerListEntry ServerListEntryPrefab;

    [Space]
    [Header("References")]
    public LoaderUI Loader;
    public Transform ServerListContainer;
    public TextMeshProUGUI PageText;
    public TextMeshProUGUI ServerNameOrderDirectionText;
    public TextMeshProUGUI PlayerCountOrderDirectionText;
    public TextMeshProUGUI NoActiveServersText;
    public RectTransform ServerNameOrderDirection;
    public RectTransform PlayerCountOrderDirection;
    public Image PreviousPageImage;
    public Image NextPageImage;

    [Space]
    [Header("Configuration")]
    public bool HideFull = true;
    public bool HidePrivate = true;
    public int Page = 0;
    public int PageSize = 4;
    public string OrderBy = "PlayerCount";
    public string OrderDirection = "desc";

    [Space]
    [Header("UI")]
    public float UpdateSpeed = 4.0f;
    public Color ActiveSortDirectionColor;
    public Color InactiveSortDirectionColor;

    private int totalPages = 0;
    private bool refreshing = false;
    private bool hideNoActiveServersText = true;
    private bool hidePageCountText = true;

    private void OnEnable()
    {
      RefreshServerList(true);
    }

    public void ToggleOrderByPlayerCount()
    {
      ServerNameOrderDirectionText.color = InactiveSortDirectionColor;
      ServerNameOrderDirection.GetComponent<Image>().color = InactiveSortDirectionColor;

      PlayerCountOrderDirectionText.color = ActiveSortDirectionColor;
      PlayerCountOrderDirection.GetComponent<Image>().color = ActiveSortDirectionColor;

      OrderBy = "PlayerCount";
      OrderDirection = OrderDirection == "desc" ? "asc" : "desc";

      RefreshServerList(true);
    }

    public void ToggleOrderByServerName()
    {
      PlayerCountOrderDirectionText.color = InactiveSortDirectionColor;
      PlayerCountOrderDirection.GetComponent<Image>().color = InactiveSortDirectionColor;

      ServerNameOrderDirectionText.color = ActiveSortDirectionColor;
      ServerNameOrderDirection.GetComponent<Image>().color = ActiveSortDirectionColor;

      OrderBy = "ServerName";
      OrderDirection = OrderDirection == "desc" ? "asc" : "desc";

      RefreshServerList(true);
    }

    public void NextPage()
    {
      if (Page + 1 < totalPages)
      {
        Page += 1;

        RefreshServerList(false);
      }
    }

    public void PreviousPage()
    {
      if (Page > 0)
      {
        Page -= 1;

        RefreshServerList(false);
      }
    }

    public void ToggleHideFull()
    {
      HideFull = !HideFull;
      RefreshServerList(true);
    }

    public void ToggleHidePrivate()
    {
      HidePrivate = !HidePrivate;
      RefreshServerList(true);
    }

    public void RefreshServerList(bool fullReset)
    {
      if (refreshing)
      {
        return;
      }

      hideNoActiveServersText = true;

      refreshing = true;

      if (fullReset)
      {
        Page = 0;
      }

      foreach (Transform t in ServerListContainer)
      {
        Destroy(t.gameObject);
      }

      PageText.gameObject.SetActive(false);
      Loader.StartLoading();

      GameServerApiClient.Instance.BrowseGameServers(RelayProjectSettings.ProjectId, HideFull, HidePrivate, Page, PageSize, OrderBy, OrderDirection, response =>
      {
        CoroutineUtility.Instance.StartCoroutine(0.5f, () =>
        {
          hideNoActiveServersText = response.TotalCount != 0;
          hidePageCountText = response.TotalCount == 0;

          totalPages = response.TotalCount / PageSize;
          PageText.gameObject.SetActive(true);
          PageText.text = $"{Page + 1} / {totalPages}";

          Loader.StopLoading();
          for (int i = 0; i < response.GameServerList.Length; i++)
          {
            var serverInfo = response.GameServerList[i];

            Instantiate(ServerListEntryPrefab, ServerListContainer).Initialize
            (
              serverInfo.IPAddress,
              serverInfo.Port,
              serverInfo.GameServerId,
              serverInfo.GameServerName,
              serverInfo.Mode,
              serverInfo.CurrentPlayerCount,
              serverInfo.MaxPlayerCapacity,
              serverInfo.Private
            );
          }
          refreshing = false;
        });
      });
    }

    private void Update()
    {
      NoActiveServersText.color = Color.Lerp(NoActiveServersText.color, new Color(1.0f, 1.0f, 1.0f, hideNoActiveServersText ? 0.0f : 1.0f), Time.deltaTime * 18);
      PageText.color = Color.Lerp(PageText.color, new Color(1.0f, 1.0f, 1.0f, hidePageCountText ? 0.0f : 1.0f), Time.deltaTime * 18);
      PreviousPageImage.color = Color.Lerp(PreviousPageImage.color, new Color(1.0f, 1.0f, 1.0f, Page > 0 ? 1.0f : 0.0f), Time.deltaTime * 18);
      NextPageImage.color = Color.Lerp(NextPageImage.color, new Color(1.0f, 1.0f, 1.0f, totalPages > 1 && Page + 1 < totalPages ? 1.0f : 0.0f), Time.deltaTime * 18);

      if (OrderBy == "PlayerCount")
      {
        PlayerCountOrderDirection.transform.eulerAngles = Vector3.Lerp(PlayerCountOrderDirection.transform.eulerAngles, Vector3.forward * (OrderDirection == "desc" ? 180 : 0), Time.deltaTime * UpdateSpeed);
      }
      else
      {
        ServerNameOrderDirection.transform.eulerAngles = Vector3.Lerp(ServerNameOrderDirection.transform.eulerAngles, Vector3.forward * (OrderDirection == "desc" ? 180 : 0), Time.deltaTime * UpdateSpeed);
      }
    }
  }
}
