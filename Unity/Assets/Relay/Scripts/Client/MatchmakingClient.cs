using System.Linq;
using UnityEngine;

namespace BitterShark.Relay
{
  public class MatchmakingClient : MonoBehaviour
  {
    [Header("Configuration")]
    public bool FindMatchOnStart = true;

    void Start()
    {
      if (FindMatchOnStart)
      {
        FindMatch(string.Empty);
      }
    }

    public void FindMatch(string tag)
    {
      if (GameServerLobbyInformationContainer.UseStatic)
      {
        MessageBusManager.Instance.Publish(new GameServerConnectionDetailsReceivedMessage()
        {
          IPAddress = GameServerLobbyInformationContainer.IPAddress,
          Port = GameServerLobbyInformationContainer.Port,
          GameServerId = GameServerLobbyInformationContainer.GameServerId
        });
      }
      else
      {

        PartyApiClient.Instance.GetPlayerParty(RelayPlayerManager.Instance.GetPlayerId(), response =>
        {
          if (response.Party != null && response.Party.Length > 0)
          {
            var partyLeaderPlayer = response.Party.FirstOrDefault(x => x.IsPartyLeader);
            if (RelayPlayerManager.Instance.GetPlayerId() != partyLeaderPlayer.PlayerId)
            {
              if (!partyLeaderPlayer.InGameServer)
              {
                Debug.LogError($"Matchmaking while in a Party but the party leader isn't in a game server");
                return;
              }

              MessageBusManager.Instance.Publish(new GameServerConnectionDetailsReceivedMessage()
              {
                GameServerId = partyLeaderPlayer.InGameServerId,
                IPAddress = partyLeaderPlayer.InGameServerIPAddress,
                Port = partyLeaderPlayer.InGameServerPort
              });

              return;
            }
          }
        });

        var projectId = RelayProjectSettings.GetProjectSettings()?.ProjectId;
        if (string.IsNullOrWhiteSpace(projectId))
        {
          projectId = "3825620d-3a42-4325-9ce9-7e0341f68cbc";
          tag = Application.identifier;
        }

        GameServerApiClient.Instance.FindGameServer(projectId, tag, response =>
        {
          if (!response.Success)
          {
            MessageBusManager.Instance.Publish(new UnsuccessfulMatchmakingMessage()
            {
              Message = response.Message
            });
          }
          else
          {
            MessageBusManager.Instance.Publish(new GameServerConnectionDetailsReceivedMessage()
            {
              GameServerId = response.GameServer.Id,
              IPAddress = response.GameServer.IPAddress,
              Port = response.GameServer.Port
            });
          }
        });
      }
    }
  }
}
