using Relay.GameServer.Core.Interfaces;
using Relay.GameServer.DataModel;

namespace Relay.GameServer.Repository.Interfaces
{
  public interface IGameServerHostRepository
  {
    UpdateGameServerHostInfoSPResponse UpdateGameServerHostInfo(IRelayUnitOfWork uw, UpdateGameServerHostInfoSPRequest request);
  }
}
