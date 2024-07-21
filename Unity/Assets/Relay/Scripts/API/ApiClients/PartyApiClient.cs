using Relay.Contracts;
using System;
using UnityEngine;

namespace BitterShark.Relay
{
  public class PartyApiClient : MonoBehaviour
  {
    private static PartyApiClient _instance;
    public static PartyApiClient Instance
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

    public void GetPlayerParty(int playerId, Action<GetPlayerPartyResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PartyApiRoute + "/", new GetPlayerPartyRequest()
      {
        PlayerId = playerId
      }, responseCallback));
    }

    public void SetPartyLeader(string playerApiKey, int partyId, int playerId, Action<SetPartyLeaderPlayerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PartyApiRoute + "/assignleader", new SetPartyLeaderPlayerRequest()
      {
        NewLeaderPlayerId = playerId,
        PartyId = partyId,
        PlayerApiKey = Guid.Parse(playerApiKey)
      }, responseCallback));
    }

    public void KickPartyPlayer(string playerApiKey, int partyId, int playerId, Action<KickPartyPlayerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PartyApiRoute + "/kick", new KickPartyPlayerRequest()
      {
        PartyId = partyId,
        PlayerApiKey = Guid.Parse(playerApiKey),
        KickPlayerId = playerId
      }, responseCallback));
    }

    public void InvitePlayerToParty(string projectId, string playerApiKey, int playerId, Action<InvitePlayerToPartyResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PartyApiRoute + "/invite", new InvitePlayerToPartyRequest()
      {
        ProjectId = projectId,
        FromPlayerApiKey = playerApiKey,
        ToPlayerId = playerId
      }, responseCallback));
    }

    public void LeaveParty(string playerApiKey, Action<LeavePartyResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.PartyApiRoute + "/leave", new LeavePartyRequest()
      {
        PlayerApiKey = playerApiKey
      }, responseCallback));
    }
  }
}
