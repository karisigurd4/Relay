using System;

namespace Relay.DataModel
{
  public class GetGameServerByCodeSPRequest
  {
    public Guid ProjectId { get; set; }
    public string Code { get; set; }
  }

  public class GetGameServerByCodeSPResponseJson
  {
    public bool Success { get; set; }
    public string ResultJson { get; set; }
  }

  public class GetGameServerByCodeSPResponse
  {
    public bool Success { get; set; }
    public GameServerListEntry GameServer { get; set; }
  }

  public static class GetGameServerByCodeSP
  {
    public static string Name => "[Relay].[GetGameServerByCode]";

    public static object CreateParameters(Guid projectId, string code) => new
    {
      projectId,
      code
    };
  }
}
