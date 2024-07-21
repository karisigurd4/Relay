namespace Relay.GameServer.DataModel
{
  using Relay.GameServer.DataModel.Types;

  public class GameServerOperationRequest
  {
    public GameServerOperationType Operation { get; set; }
    public int RequestPort { get; set; }
    public int RequestGameServerId { get; set; }
    public string RequestProjectId { get; set; }
  }

  public class PollGameServerOperationsSPRequest
  {
    public string HostName { get; set; }
  }

  public class PollGameServerOperationsSPResponseJson
  {
    public string OperationRequestsJson { get; set; }
    public bool Success { get; set; }
  }

  public class PollGameServerOperationsSPResponse
  {
    public GameServerOperationRequest[] OperationRequests { get; set; }
    public bool Success { get; set; }
  }

  public static class PollGameServerOperationsSP
  {
    public static string Name => "[Relay].[PollGameServerOperationRequests]";

    public static object CreateParameters(string hostName) => new
    {
      hostName
    };
  }
}
