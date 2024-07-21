using UnityEngine;
#if UNITY_EDITOR
#endif

namespace BitterShark.Relay
{
  public class RelayPlayerManager : MonoBehaviour
  {
    public static string PlayerApiKey => Instance.GetPlayerApiKey();
    public static int PlayerId => Instance.GetPlayerId();

    private static RelayPlayerManager _instance;
    public static RelayPlayerManager Instance
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

    public static bool ForceClonePlayer = false;

    private static string playerRegisteredKey = "_RELAY_PREF_REGISTERED_KEY";
    private static string playerIdKey = "_RELAY_PREF_ID_KEY";
    private static string playerApiKey = "_RELAY_PREF_APIKEY_KEY";
    private static string playerNameKey = "_RELAY_PREF_PLAYERNAME_KEY";

    public int CurrentGameServerId = 0;

    public int GetPlayerId()
    {
      return PlayerPrefs.GetInt(playerIdKey);
    }

    public string GetPlayerApiKey()
    {
      return PlayerPrefs.GetString(playerApiKey);
    }

    public string GetPlayerName()
    {
      return PlayerPrefs.GetString(playerNameKey);
    }

    public bool IsPlayerRegistered()
    {
      return PlayerPrefs.HasKey(playerRegisteredKey);
    }

    public void ClearPlayerData()
    {
      PlayerPrefs.DeleteKey(playerIdKey);
      PlayerPrefs.DeleteKey(playerApiKey);
      PlayerPrefs.DeleteKey(playerNameKey);
      PlayerPrefs.DeleteKey(playerRegisteredKey);
    }

    void Start()
    {
      if (ForceClonePlayer)
      {
        playerRegisteredKey = "_RELAY_PREF_REGISTERED_KEY_CLONE";
        playerIdKey = "_RELAY_PREF_ID_KEY_CLONE";
        playerApiKey = "_RELAY_PREF_APIKEY_KEY_CLONE";
        playerNameKey = "_RELAY_PREF_PLAYERNAME_KEY_CLONE";
      }

      if (!PlayerPrefs.HasKey(playerRegisteredKey))
      {
        CreateGeneratedPlayer();
      }
      else
      {
        PlayerApiClient.Instance.GetPlayer(RelayProjectSettings.ProjectId, GetPlayerId(), GetPlayerApiKey(), response =>
        {
          if (response == null || !response.Success || response.Player == null)
          {
            Debug.Log($"Refreshing player");
            ClearPlayerData();
            CreateGeneratedPlayer();
          }
        });
      }
    }

    private static void CreateGeneratedPlayer()
    {
      var generatedPlayerName = $"Player{Random.Range(10000, 99999)}";

      var projectId = RelayProjectSettings.GetProjectSettings()?.ProjectId;
      if (string.IsNullOrWhiteSpace(projectId))
      {
        return;
      }

      PlayerApiClient.Instance.RegisterPlayer(projectId, generatedPlayerName, (registerPlayerResponse) =>
      {
        PlayerPrefs.SetInt(playerIdKey, registerPlayerResponse.Id);
        PlayerPrefs.SetString(playerApiKey, registerPlayerResponse.ApiKey);
        PlayerPrefs.SetString(playerNameKey, generatedPlayerName);
        PlayerPrefs.SetInt(playerRegisteredKey, 1);

        MessageBusManager.Instance.Publish(new PlayerRegisteredMessage()
        {
          PlayerApiKey = registerPlayerResponse.ApiKey,
          PlayerId = registerPlayerResponse.Id,
          PlayerName = generatedPlayerName
        });
      });
    }
  }
}
