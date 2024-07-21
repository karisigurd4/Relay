using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class LobbyUIWatcher : MonoBehaviour
  {
    private static LobbyUIWatcher _instance;
    public static LobbyUIWatcher Instance
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

    [Header("References")]
    public Transform JoinedPlayersTransformReference;
    public TextMeshProUGUI JoinedPlayersCountTxt;

    [Header("Prefabs")]
    public LobbyPlayerUI LobbyPlayerUIPRefab;

    private List<int> lobbyPlayerApiIds = new List<int>();
    private Dictionary<int, LobbyPlayerUI> lobbyPlayerUIs = new Dictionary<int, LobbyPlayerUI>();

    private float lastPoll = 0.0f;
    private int gameServerId;
    private bool receivedGameServerId = false;

    public void SetGameServerId(int id)
    {
      gameServerId = id;
      receivedGameServerId = true;
      JoinedPlayersCountTxt.text = "0";
    }

    void Update()
    {
      if (receivedGameServerId)
      {
        if (Time.time > lastPoll + 1.0f)
        {
          lastPoll = Time.time;

          GameServerApiClient.Instance.GetGameServerInfo(gameServerId, response =>
          {
            var newPlayers = response.Players.Where(x => !lobbyPlayerApiIds.Contains(x.PlayerId)).ToArray();
            for (int i = 0; i < newPlayers.Length; i++)
            {
              lobbyPlayerApiIds.Add(newPlayers[i].PlayerId);
              var joinedPlayerUI = Instantiate(LobbyPlayerUIPRefab, JoinedPlayersTransformReference);
              joinedPlayerUI.Initialize(newPlayers[i].PlayerName);
              lobbyPlayerUIs.Add(newPlayers[i].PlayerId, joinedPlayerUI);
            }

            JoinedPlayersCountTxt.text = lobbyPlayerApiIds.Count.ToString();
          });
        }
      }
    }
  }
}
