using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class ServerListEntry : MonoBehaviour
  {
    [Header("SceneConfigurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("References")]
    public TextMeshProUGUI ServerNameText;
    public TextMeshProUGUI ServerTagsText;
    public TextMeshProUGUI PlayerCountText;
    public GameObject PrivateImage;

    private string ipAddress;
    private int port;
    private int gameServerId;
    private bool isPrivate;

    public void Initialize(string ipAddress, int port, int gameServerId, string serverName, string serverTags, int playerCount, int maximumPlayerCapacity, bool isPrivate)
    {
      this.ipAddress = ipAddress;
      this.port = port;
      this.gameServerId = gameServerId;
      this.isPrivate = isPrivate;

      ServerNameText.text = serverName;
      ServerTagsText.text = serverTags;
      PlayerCountText.text = $"{playerCount}/{maximumPlayerCapacity} Players";
      PrivateImage.SetActive(isPrivate);
    }

    public void OnClick()
    {
      if (isPrivate)
      {
        MessageBusManager.Instance.Publish(new ShowJoinWithCodeWindowMessage() { });
        return;
      }

      MessageBusManager.Instance.Publish(new ShowConfirmationDialogMessage()
      {
        Text = "Join game?",
        OnConfirmAction = () =>
        {
          MessageBusManager.Instance.Publish(new ShowLoaderDialogMessage()
          {
            LoadingText = "Joining game server"
          });

          var modeConfig = MainMenuOverlay.Instance.GameModeConfigurations.FirstOrDefault(x => x.ModeName == ServerTagsText.text);

          if (modeConfig == null)
          {
            Debug.LogError($"Could not fetch mode config");
            MessageBusManager.Instance.Publish(new HideLoaderDialogMessage() { });
            return;
          }

          RelaySceneLoaderMemory.LoadSceneName = Path.GetFileName(modeConfig.SceneName).Replace(".unity", "");

          GameServerLobbyInformationContainer.UseStatic = true;
          GameServerLobbyInformationContainer.IPAddress = ipAddress;
          GameServerLobbyInformationContainer.Port = port;
          GameServerLobbyInformationContainer.GameServerId = gameServerId;

          RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
        }
      });
    }
  }
}
