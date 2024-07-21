using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class HostGameWindow : MonoBehaviour
  {
    [Header("SceneConfigurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("References")]
    public TMP_InputField ServerNameText;
    public TextMeshProUGUI CodeText;
    public CanvasGroup CodeCanvasGroup;
    public TMP_Dropdown MapModeDropdown;
    public TextMeshProUGUI MaxPlayersText;

    private bool isPrivate = false;
    private int maxPlayers = 8;
    private int selectedModeIndex = 0;

    private void Start()
    {
      if (!MainMenuOverlay.Instance.AllowMultipleModes)
      {
        MapModeDropdown.transform.parent.gameObject.SetActive(false);
      }
      else
      {
        MapModeDropdown.AddOptions(MainMenuOverlay.Instance.GameModeConfigurations.Select(x => x.ModeName).ToList());
      }
      MapModeDropdown.onValueChanged.AddListener(delegate
      {
        DropdownValueChanged(MapModeDropdown);
      });

      var modeConfig = MainMenuOverlay.Instance
       .GameModeConfigurations
       .FirstOrDefault();

      if (modeConfig == null)
      {
        Debug.LogError($"Error fetching mode configuration");
        return;
      }

      RelaySceneLoaderMemory.LoadSceneName = Path.GetFileName(modeConfig.SceneName).Replace(".unity", "");
      MaxPlayersText.text = modeConfig.MaxPossiblePlayers.ToString();
    }

    private void OnEnable()
    {
      PlayerApiClient.Instance.GetPlayer(RelayProjectSettings.GetProjectSettings().ProjectId, RelayPlayerManager.PlayerId, null, response =>
      {
        if (response == null || !response.Success)
        {
          MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
          {
            LeftHeaderText = "Service error",
            RightHeaderText = "",
            InformationData = "Error when fetching player info"
          });
          return;
        }

        if (response.Success)
        {
          ServerNameText.text = response.Player.Name + "'s Server";
        }
      });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
      var modeConfig = MainMenuOverlay.Instance
       .GameModeConfigurations
       .FirstOrDefault(x => x.ModeName == MapModeDropdown.options[MapModeDropdown.value].text);

      if (modeConfig == null)
      {
        Debug.LogError($"Error fetching mode configuration");
        return;
      }

      Debug.LogError($"Error fetching mode configuration");

      RelaySceneLoaderMemory.LoadSceneName = modeConfig.SceneName;
      MaxPlayersText.text = modeConfig.MaxPossiblePlayers.ToString();
    }

    public void IncreaseMaxPlayerCount()
    {
      var modeConfigMaxPlayers = MainMenuOverlay.Instance
        .GameModeConfigurations
        .FirstOrDefault(x => x.ModeName == MapModeDropdown.options[MapModeDropdown.value].text).MaxPossiblePlayers;

      if (maxPlayers < modeConfigMaxPlayers)
      {
        maxPlayers += 1;
        MaxPlayersText.text = maxPlayers.ToString();
      }
    }

    public void DecreaseMaxPlayersCount()
    {
      if (maxPlayers > 2)
      {
        maxPlayers -= 1;
        MaxPlayersText.text = maxPlayers.ToString();
      }
    }

    public void TogglePrivate()
    {
      isPrivate = !isPrivate;

      if (isPrivate)
      {
        GenerateServerCode();
      }
    }

    public void GenerateServerCode()
    {
      string code = "";

      for (int i = 0; i < 3; i++)
      {
        code += (char)(Random.Range(66, 90));
      }

      for (int i = 0; i < 3; i++)
      {
        code += Random.Range(0, 9);
      }

      CodeText.text = code;
    }

    public void StartGameServer()
    {
      MessageBusManager.Instance.Publish(new ShowLoaderDialogMessage()
      {
        LoadingText = "Starting game server"
      });

      var modeConfig = MainMenuOverlay.Instance
       .GameModeConfigurations
       .FirstOrDefault(x => x.ModeName == MapModeDropdown.options[MapModeDropdown.value].text);

      if (modeConfig == null)
      {
        Debug.LogError($"Error fetching mode configuration");
        return;
      }

      GameServerApiClient.Instance.HostGameServer
      (
        RelayProjectSettings.ProjectId,
        ServerNameText.text,
        isPrivate,
        CodeText.text,
        modeConfig.ModeName,
        maxPlayers,
        response =>
        {
          //MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
          //{
          //  InformationData = "Could not start game server\n" + response.Message,
          //  LeftHeaderText = "Service error",
          //  RightHeaderText = ""
          //});
          //return;

          GameServerLobbyInformationContainer.UseStatic = true;
          GameServerLobbyInformationContainer.IPAddress = response.IPAddress;
          GameServerLobbyInformationContainer.Port = response.Port;
          GameServerLobbyInformationContainer.GameServerId = response.GameServerId;

          RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
        }
      );
    }

    private void Update()
    {
      CodeCanvasGroup.alpha = Mathf.Lerp(CodeCanvasGroup.alpha, isPrivate ? 1.0f : 0.0f, Time.deltaTime * 18);
    }
  }
}