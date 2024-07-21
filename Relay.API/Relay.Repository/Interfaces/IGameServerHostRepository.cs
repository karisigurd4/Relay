using Relay.Core.Interfaces;
using Relay.DataModel;

namespace Relay.Repository.Interfaces
{
  public interface IGameServerHostRepository
  {
    public GetGameServerHostsSPResponse GetGameServerHosts(IRelayUnitOfWork uw, GetGameServerHostsSPRequest request);
  }
}
