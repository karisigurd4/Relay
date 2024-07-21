using AutoMapper;
using Relay.Contracts;
using Relay.Core.Factories;
using Relay.Implementation.Interfaces;
using Relay.Repository.Interfaces;

namespace Relay.Implementation.Implementation
{
  public class GameServerHostManager : IGameServerHostManager
  {
    private readonly IMapper mapper;
    private readonly IGameServerHostRepository gameServerHostRepository;

    public GameServerHostManager(IMapper mapper, IGameServerHostRepository gameServerHostRepository)
    {
      this.mapper = mapper;
      this.gameServerHostRepository = gameServerHostRepository;
    }

    public GetGameServerHostsResponse GetGameServerHosts(GetGameServerHostsRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetGameServerHostsResponse>(gameServerHostRepository.GetGameServerHosts(uw, new DataModel.GetGameServerHostsSPRequest()
        {
        }));
      }
    }
  }
}
