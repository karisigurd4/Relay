namespace Relay.GameServer.DataModel
{
  public static class UpdateGameObjectStateResponseFactory
  {
    public static UpdateGameObjectStateResponse Create(UpdateGameObjectStateResult result) => new UpdateGameObjectStateResponse()
    {
      Success = true,
      Result = result
    };
  }
}
