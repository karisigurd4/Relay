using Relay.Contracts;

namespace Relay.Implementation.Interfaces
{
  public interface IGameServerHostManager
  {
    GetGameServerHostsResponse GetGameServerHosts(GetGameServerHostsRequest request);
  }
}
