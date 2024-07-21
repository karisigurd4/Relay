namespace Relay.GameServer.DataModel
{
  public enum UpdateGameObjectStateResult
  {
    Added,
    Updated
  }

  public class UpdateGameObjectStateResponse
  {
    public UpdateGameObjectStateResult Result;
    public bool Success;
  }
}
