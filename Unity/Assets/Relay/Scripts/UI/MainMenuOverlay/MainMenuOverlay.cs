using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitterShark.Relay
{
  public class MainMenuOverlay : MonoBehaviour
  {
    private static MainMenuOverlay _instance;
    public static MainMenuOverlay Instance
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

    [SerializeField]
    public string InGameSceneName;
    [SerializeField]
    public int SingleGameModeMaxPlayers = 16;
    [SerializeField]
    public LobbySystemType LobbySystemType;
    [SerializeField]
    public bool AllowMultipleModes = true;
    [SerializeField]
    public List<RelayGameModeConfiguration> GameModeConfigurations = new List<RelayGameModeConfiguration>();

    public RelayGameModeConfiguration GetGameModeConfiguration(string gameModeName)
    {
      return GameModeConfigurations.FirstOrDefault(x => x.ModeName == gameModeName);
    }
  }
}