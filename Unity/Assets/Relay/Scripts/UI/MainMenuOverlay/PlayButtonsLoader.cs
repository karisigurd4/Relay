using UnityEngine;

namespace BitterShark.Relay
{

  public class PlayButtonsLoader : MonoBehaviour
  {
    [Header("References")]
    public GameObject MatchmakingPlayButton;
    public GameObject GameServerBrowserButtons;

    private void Start()
    {
      if (MainMenuOverlay.Instance.LobbySystemType == LobbySystemType.GameServerBrowser)
      {
        MatchmakingPlayButton.SetActive(false);
        GameServerBrowserButtons.SetActive(true);
      }
      else
      {
        MatchmakingPlayButton.SetActive(true);
        GameServerBrowserButtons.SetActive(false);
      }
    }
  }
}