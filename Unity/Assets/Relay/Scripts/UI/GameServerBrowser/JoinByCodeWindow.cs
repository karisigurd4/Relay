using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class JoinByCodeWindow : MonoBehaviour
  {
    [Header("SceneConfigurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("References")]
    public TextMeshProUGUI CodeText;

    public void ClickJoin()
    {
      MessageBusManager.Instance.Publish(new ShowLoaderDialogMessage()
      {
        LoadingText = "Joining server"
      });

      GameServerApiClient.Instance.GetGameServerByCode(RelayProjectSettings.ProjectId, CodeText.text.Replace("\u200B", ""), response =>
      {
        var modeConfig = MainMenuOverlay.Instance.GameModeConfigurations.FirstOrDefault(x => x.ModeName == response.GameServer.Mode);

        if (modeConfig == null)
        {
          Debug.LogError($"Could not fetch mode config");
          MessageBusManager.Instance.Publish(new HideLoaderDialogMessage() { });
          return;
        }

        RelaySceneLoaderMemory.LoadSceneName = Path.GetFileName(modeConfig.SceneName).Replace(".unity", "");

        GameServerLobbyInformationContainer.UseStatic = true;
        GameServerLobbyInformationContainer.IPAddress = response.GameServer.IPAddress;
        GameServerLobbyInformationContainer.Port = response.GameServer.Port;
        GameServerLobbyInformationContainer.GameServerId = response.GameServer.GameServerId;

        RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
      });
    }
  }
}
