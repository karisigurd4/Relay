using Relay.Contracts;
using System;
using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerApiClient : MonoBehaviour
  {
    private static GameServerApiClient _instance;
    public static GameServerApiClient Instance
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

    public void FindGameServer(string projectId, string tag, Action<FindGameServerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.GameServerApiRoute + "/matchmaking", new FindGameServerRequest()
      {
        ProjectId = projectId,
        Tag = tag
      }, responseCallback));
    }

    public void GetGameServerInfo(int gameServerId, Action<GetGameServerInfoByIdResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.GameServerApiRoute + "/info", new GetGameServerInfoByIdRequest()
      {
        GameServerId = gameServerId
      }, responseCallback));
    }

    public void BrowseGameServers(string projectId, bool hideFull, bool hidePrivate, int page, int pageSize, string orderBy, string orderDirection, Action<BrowseGameServersResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.GameServerApiRoute + "/browse", new BrowseGameServersRequest()
      {
        HideFull = hideFull,
        HidePrivate = hidePrivate,
        OrderBy = orderBy,
        OrderDirection = orderDirection,
        Page = page,
        PageSize = pageSize,
        ProjectId = projectId
      }, responseCallback));
    }

    public void GetGameServerByCode(string projectId, string code, Action<GetGameServerByCodeResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.GameServerApiRoute + "/code", new GetGameServerByCodeRequest()
      {
        Code = code,
        ProjectId = projectId
      }, responseCallback));
    }

    public void HostGameServer(string projectId, string serverName, bool isPrivate, string code, string mode, int maxPlayers, Action<HostGameServerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Post(ApiConfiguration.Instance.ApiRoute + Constants.GameServerApiRoute + "/host", new HostGameServerRequest()
      {
        Code = code,
        IsPrivate = isPrivate,
        MaxPlayers = maxPlayers,
        Mode = mode,
        ProjectId = projectId,
        ServerName = serverName
      }, responseCallback));
    }
  }
}
