namespace Relay.DataModel
{
  public class BrowseGameServersSPRequest
  {
    public string ProjectId { get; set; }
    public bool HideFull { get; set; }
    public bool HidePrivate { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string OrderBy { get; set; }
    public string OrderDirection { get; set; }
  }

  public class BrowseGameServersSPResponseJson
  {
    public bool Success { get; set; }
    public string GameServerListJson { get; set; }
    public int TotalCount { get; set; }
  }

  public class BrowseGameServersSPResponse
  {
    public bool Success { get; set; }
    public GameServerListEntry[] GameServerList { get; set; }
    public int TotalCount { get; set; }
  }

  public static class BrowseGameServersSP
  {
    public static string Name => "[Relay].[BrowseGameServers]";

    public static object CreateParameters(string projectId, bool hideFull, bool hidePrivate, int page, int pageSize, string orderBy, string orderDirection) => new
    {
      projectId,
      hideFull,
      hidePrivate,
      page,
      pageSize,
      orderBy,
      orderDirection
    };
  }
}
