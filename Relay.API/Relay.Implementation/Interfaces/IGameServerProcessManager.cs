using Relay.Contracts;

namespace Relay.Implementation.Interfaces
{
  public interface IGameServerProcessManager
  {
    StartGameServerProcessResponse StartGameServerProcess(StartGameServerProcessRequest request);
    StopGameServerProcessResponse StopGameServerProcess(StopGameServerProcessRequest request);
  }
}
