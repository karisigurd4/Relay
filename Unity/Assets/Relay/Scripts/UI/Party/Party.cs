using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class Party : MonoBehaviour
  {
    [Header("Prefabs")]
    public PartySlot PartySlotPrefab;
    public Sprite DefaultPlayerSprite;

    [Header("Scene configurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("References")]
    public Transform PartySlotsTransform;
    public GameObject LeaveButton;
    public TextMeshProUGUI WaitingForPartyLeaderTxtReference;
    public GameObject PlayButtonReference;

    private static float refreshRate = .5f;
    private float lastPolledPartyTime = 0.0f;
    private int partyLeaderId = 0;

    private Dictionary<int, PartySlot> partyPlayerIds = new Dictionary<int, PartySlot>();

    private void Start()
    {
      LeaveButton.SetActive(false);
    }

    public void LeaveParty()
    {
      PartyApiClient.Instance.LeaveParty(RelayPlayerManager.Instance.GetPlayerApiKey(), response =>
      {
        LeaveButton.SetActive(false);

        foreach (var key in partyPlayerIds.Keys)
        {
          Destroy(partyPlayerIds[key].gameObject);
        }

        partyPlayerIds.Clear();

        MessageBusManager.Instance.Publish(new PlayerLeftPartyMessage()
        {
          PlayerId = RelayPlayerManager.Instance.GetPlayerId()
        });
      });
    }

    private void Update()
    {
      if (Time.time > lastPolledPartyTime + refreshRate)
      {
        lastPolledPartyTime = Time.time;

        PartyApiClient.Instance.GetPlayerParty(RelayPlayerManager.Instance.GetPlayerId(), response =>
        {
          if (response == null || response.Party == null || response.Party.Length == 0)
          {
            if (!PlayButtonReference.activeInHierarchy)
            {
              PlayButtonReference.SetActive(true);
            }

            if (WaitingForPartyLeaderTxtReference.gameObject.activeInHierarchy)
            {
              WaitingForPartyLeaderTxtReference.gameObject.SetActive(false);
            }

            return;
          }

          var partyLeaderPlayer = response.Party.FirstOrDefault(x => x.IsPartyLeader);
          if (partyLeaderPlayer != null)
          {
            partyLeaderId = partyLeaderPlayer.PlayerId;

            if (partyLeaderPlayer.PlayerId != RelayPlayerManager.Instance.GetPlayerId())
            {
              if (PlayButtonReference.activeInHierarchy)
              {
                PlayButtonReference.SetActive(false);
              }

              if (!WaitingForPartyLeaderTxtReference.gameObject.activeInHierarchy)
              {
                WaitingForPartyLeaderTxtReference.gameObject.SetActive(true);
              }

              if (partyLeaderPlayer.InGameServer)
              {
                RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
              }
            }
          }

          if (!LeaveButton.activeInHierarchy)
          {
            LeaveButton.SetActive(true);
          }

          var newPlayers = response.Party.Where(x => !partyPlayerIds.ContainsKey(x.PlayerId)).ToArray();
          for (int i = 0; i < newPlayers.Length; i++)
          {
            AddPlayer(newPlayers[i].PlayerId, newPlayers[i].PlayerName, newPlayers[i].IsPartyLeader);
          }

          var leftPLayers = partyPlayerIds.Keys.Where(x => !response.Party.Select(x => x.PlayerId).Contains(x)).ToArray();
          for (int i = 0; i < leftPLayers.Length; i++)
          {
            RemovePlayer(leftPLayers[i], partyLeaderId == leftPLayers[i]);
          }
        });
      }
    }

    private void RemovePlayer(int playerId, bool isPartyLeader)
    {
      Destroy(partyPlayerIds[playerId].gameObject);
      partyPlayerIds.Remove(playerId);

      if (isPartyLeader)
      {
        LeaveParty();
      }

      MessageBusManager.Instance.Publish(new PlayerLeftPartyMessage()
      {
        PlayerId = playerId
      });
    }

    private void AddPlayer(int playerId, string playerName, bool isPartyLeader)
    {
      var slot = Instantiate(PartySlotPrefab, PartySlotsTransform);
      slot.transform.SetSiblingIndex(0);
      slot.Populate(DefaultPlayerSprite, playerName, isPartyLeader);

      partyPlayerIds.Add(playerId, slot);

      MessageBusManager.Instance.Publish(new PlayerJoinedPartyMessage()
      {
        PlayerId = playerId,
        PlayerName = playerName,
        PlayerProfilePicture = null
      });
    }
  }
}
