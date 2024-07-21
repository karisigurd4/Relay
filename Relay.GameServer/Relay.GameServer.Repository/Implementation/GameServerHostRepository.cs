using Relay.GameServer.Core.Interfaces;
using Relay.GameServer.DataModel;
using Relay.GameServer.Repository.Interfaces;

namespace Relay.GameServer.Repository.Implementation
{
  public class GameServerHostRepository : IGameServerHostRepository
  {
    public UpdateGameServerHostInfoSPResponse UpdateGameServerHostInfo(IRelayUnitOfWork uw, UpdateGameServerHostInfoSPRequest request)
    {
      return uw.ExecuteSP<UpdateGameServerHostInfoSPResponse>
      (
        UpdateGameServerHostInfoSP.Name,
        UpdateGameServerHostInfoSP.CreateParameters(request.HostName, request.CpuUsage, request.MemoryUsage)
      );
    }
  }
}
