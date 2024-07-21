namespace Relay.DataModel
{
  public class AddGameServerOperationRequestSPRequest
  {
    public GameServerOperationType Operation { get; set; }
    public int Port { get; set; }
    public int GameServerId { get; set; }
    public string ProjectId { get; set; }
    public string GameServerHost { get; set; }
  }

  public class AddGameServerOperationRequestSPResponse
  {
    public bool Success { get; set; }
  }

  public static class AddGameServerOperationRequestSP
  {
    public static string Name => "[Relay].[AddGameServerOperationRequest]";

    public static object CreateParameters(string gameServerHost, string operation, int port, int gameServerId, string projectId) => new
    {
      gameServerHost,
      operation,
      port,
      gameServerId,
      projectId = projectId
    };
  }
}
