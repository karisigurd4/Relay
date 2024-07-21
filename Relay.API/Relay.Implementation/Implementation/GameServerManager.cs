namespace Relay.Implementation.Implementation
{
  using DataModel;
  using global::AutoMapper;
  using Relay.Contracts;
  using Relay.Core.Factories;
  using Relay.Implementation.Interfaces;
  using Relay.Repository.Interfaces;
  using System;

  public class GameServerManager : IGameServerManager
  {
    private readonly IMapper mapper;
    private readonly IGameServerRepository gameServerRepository;
    private readonly IGameServerProcessManager gameServerProcessManager;

    public GameServerManager(IMapper mapper, IGameServerRepository gameServerRepository, IGameServerProcessManager gameServerProcessManager)
    {
      this.mapper = mapper;
      this.gameServerRepository = gameServerRepository;
      this.gameServerProcessManager = gameServerProcessManager;
    }

    public AddGameServerPlayerResponse AddGameServerPlayer(AddGameServerPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<AddGameServerPlayerResponse>(gameServerRepository.AddGameServerPlayer(uw, mapper.Map<AddGameServerPlayerSPRequest>(request)));
      }
    }

    public BrowseGameServersResponse BrowseGameServers(BrowseGameServersRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<BrowseGameServersResponse>(gameServerRepository.BrowseGameServers(uw, mapper.Map<BrowseGameServersSPRequest>(request)));
      }
    }

    public FindGameServerResponse FindGameServer(FindGameServerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var findGameServerResponse = mapper.Map<FindGameServerResponse>(gameServerRepository.FindGameServer(uw, mapper.Map<FindGameServerSPRequest>(request)));
        if (!findGameServerResponse.Success)
        {
          return FindGameServerResponseFactory.Create(false, "Unexpected error occurred when querying available game servers");
        }

        if (findGameServerResponse.GameServer != null)
        {
          return findGameServerResponse;
        }
      }

      var startNewGameServerResponse = gameServerProcessManager.StartGameServerProcess(new StartGameServerProcessRequest()
      {
        ProjectId = request.ProjectId,
        Tag = request.Tag
      });

      if (!startNewGameServerResponse.Success)
      {
        return FindGameServerResponseFactory.Create(false, startNewGameServerResponse.Message);
      }

      return new FindGameServerResponse()
      {
        GameServer = new Contracts.GameServer()
        {
          Id = startNewGameServerResponse.GameServerId,
          IPAddress = startNewGameServerResponse.IPAddress,
          Port = startNewGameServerResponse.Port,
          StartDate = DateTime.UtcNow
        },
        Success = true
      };
    }

    public GetGameServerByCodeResponse GetGameServerByCode(GetGameServerByCodeRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetGameServerByCodeResponse>(gameServerRepository.GetGameServerByCode(uw, mapper.Map<GetGameServerByCodeSPRequest>(request)));
      }
    }

    public GetGameServerInfoByIdResponse GetGameServerInfoById(GetGameServerInfoByIdRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetGameServerInfoByIdResponse>(gameServerRepository.GetGameServerInfoById(uw, mapper.Map<GetGameServerInfoByIdSPRequest>(request)));
      }
    }

    public GetGameServerStatisticsResponse GetGameServerStatistics(GetGameServerStatisticsRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetGameServerStatisticsResponse>(gameServerRepository.GetGameServerStatistics(uw, mapper.Map<GetGameServerStatisticsSPRequest>(request)));
      }
    }

    public HostGameServerResponse HostGameServer(HostGameServerRequest request)
    {
      var startNewGameServerResponse = gameServerProcessManager.StartGameServerProcess(new StartGameServerProcessRequest()
      {
        ProjectId = request.ProjectId,
        Tag = request.Mode
      });

      if (!startNewGameServerResponse.Success)
      {
        return new HostGameServerResponse()
        {
          Success = false,
          Message = startNewGameServerResponse.Message
        };
      }

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var registerConfigurationResponse = gameServerRepository.RegisterGameServerConfiguration(uw, new RegisterGameServerConfigurationSPRequest()
        {
          Code = request.Code,
          GameServerId = startNewGameServerResponse.GameServerId,
          IsPrivate = request.IsPrivate,
          MaxPlayers = request.MaxPlayers,
          Mode = request.Mode,
          ServerName = request.ServerName
        });

        if (!registerConfigurationResponse.Success)
        {
          return new HostGameServerResponse()
          {
            Success = false,
            Message = "Could not register configuration"
          };
        }
      }

      return new HostGameServerResponse()
      {
        IPAddress = startNewGameServerResponse.IPAddress,
        Port = startNewGameServerResponse.Port,
        GameServerId = startNewGameServerResponse.GameServerId,
        Success = true
      };
    }

    public RemoveGameServerPlayerResponse RemoveGameServerPlayer(RemoveGameServerPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<RemoveGameServerPlayerResponse>(gameServerRepository.RemoveGameServerPlayer(uw, mapper.Map<RemoveGameServerPlayerSPRequest>(request)));
      }
    }

    public StopGameServerResponse StopGameServer(StopGameServerRequest request)
    {
      var stopGameServerProcessResponse = gameServerProcessManager.StopGameServerProcess(new StopGameServerProcessRequest()
      {
        GameServerId = request.GameServerId
      });

      return new StopGameServerResponse()
      {
        Success = stopGameServerProcessResponse.Success,
        Message = stopGameServerProcessResponse.Message
      };
    }
  }
}
