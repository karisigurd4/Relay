namespace Relay.DataModel
{
  public class FindGameServerSPResponseJson
  {
    public bool Success { get; set; }
    public string GameServerJson { get; set; }
  }

  public class FindGameServerSPRequest
  {
    public string ProjectId { get; set; }
    public string Tag { get; set; }
  }

  public static class FindGameServerSP
  {
    public static string Name => "[Relay].[FindGameServer]";

    public static object CreateParameters(string projectId, string tag) => new
    {
      projectId,
      tag
    };
  }
}
