namespace Relay.GameServer.OperationListener.Interfaces
{
  public interface IListener
  {
    void Start();
    void PerformActiveServersHealthCheck();
  }
}
