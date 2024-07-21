using AutoMapper;
using Relay.Core.Interfaces;
using Relay.DataModel;
using Relay.Repository.Interfaces;

namespace Relay.Repository.Implementation
{
  public class GameServerHostRepository : IGameServerHostRepository
  {
    private readonly IMapper mapper;

    public GameServerHostRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public GetGameServerHostsSPResponse GetGameServerHosts(IRelayUnitOfWork uw, GetGameServerHostsSPRequest request)
    {
      var jsonResponse = uw.ExecuteSP<GetGameServerHostsSPResponseJson>
      (
        GetGameServerHostsSP.Name,
        GetGameServerHostsSP.CreateParameters()
      );

      return mapper.Map<GetGameServerHostsSPResponse>(jsonResponse);
    }
  }
}
