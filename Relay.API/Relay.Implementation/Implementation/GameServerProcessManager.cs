namespace Relay.Implementation.Implementation
{
  using MassTransit;
  using Relay.Contracts;
  using Relay.Contracts.Messages;
  using Relay.Core.Factories;
  using Relay.Core.Utilities;
  using Relay.DataModel;
  using Relay.GameServer.DataModel;
  using Relay.Implementation.Interfaces;
  using Relay.Repository.Interfaces;
  using System;
  using System.Diagnostics;
  using System.Linq;

  public class GameServerProcessManager : IGameServerProcessManager
  {
    private readonly IGameServerRepository gameServerRepository;
    private readonly IGameServerHostRepository gameServerHostRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IBus bus;

    public GameServerProcessManager
    (
      IGameServerRepository gameServerRepository,
      IGameServerHostRepository gameServerHostRepository,
      IProjectRepository projectRepository,
      IBus bus
    )
    {
      this.gameServerRepository = gameServerRepository;
      this.gameServerHostRepository = gameServerHostRepository;
      this.projectRepository = projectRepository;
      this.bus = bus;
    }

    public StartGameServerProcessResponse StartGameServerProcess(StartGameServerProcessRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var projectSubscription = projectRepository.GetProjectSubscription(uw, new GetProjectSubscriptionSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId)
        });

        if (projectSubscription == null || !projectSubscription.Success)
        {
          return new StartGameServerProcessResponse()
          {
            Success = false,
            Message = "Invalid Project Id"
          };
        }

        if (projectSubscription.ProjectSubsciption.CheckExpirationFlag)
        {
          if (!projectSubscription.ProjectSubsciption.Active)
          {
            PublishProjectStatusExpiringMessage(projectSubscription);

            return new StartGameServerProcessResponse()
            {
              Success = false,
              Message = "The subscription assigned to the project is inactive"
            };
          }

          if ((DateTime.UtcNow - projectSubscription.ProjectSubsciption.LastCheckedStatusDate).Days > 7)
          {
            PublishProjectStatusExpiringMessage(projectSubscription);
          }
        }

        var hosts = gameServerHostRepository.GetGameServerHosts(uw, new GetGameServerHostsSPRequest()
        {
        });

        var host = hosts
          .Hosts
          .OrderBy(x => x.CpuUsage)
          .FirstOrDefault(x => x.Status == DataModel.GameServerHostStatus.Up);

        if (host == null)
        {
          return new StartGameServerProcessResponse()
          {
            Success = false,
            Message = "All game server hosts seem to be down",
            OperationResult = OperationResult.Failed_InternalError
          };
        }

        var getAvailablePortResponse = gameServerRepository.GetAvailableGameServerPort(uw, new GetAvailableGameServerPortSPRequest()
        {
          GameServerHost = host.HostName
        });

        var addGameServerResponse = gameServerRepository.AddGameServer(uw, new AddGameServerSPRequest()
        {
          HostName = host.HostName,
          IPAddress = ConfigurationReader.GetEnvironmentConfiguration().IPAddress,
          Port = getAvailablePortResponse.PortNumber,
          ProjectId = request.ProjectId,
          Tag = request.Tag
        });

        gameServerRepository.AddGameServerOperationRequest(uw, new AddGameServerOperationRequestSPRequest()
        {
          GameServerHost = host.HostName,
          GameServerId = addGameServerResponse.GameServerId,
          Operation = GameServerOperationType.Start,
          Port = getAvailablePortResponse.PortNumber,
          ProjectId = request.ProjectId
        });

        return new StartGameServerProcessResponse()
        {
          GameServerId = addGameServerResponse.GameServerId,
          Success = true,
          IPAddress = ConfigurationReader.GetEnvironmentConfiguration().IPAddress,
          Port = getAvailablePortResponse.PortNumber
        };
      }
    }

    private void PublishProjectStatusExpiringMessage(GetProjectSubscriptionSPResponse projectSubscription)
    {
      bus.Publish(new ProjectSubscriptionStatusExpiringMessage()
      {
        ProjectApiKey = projectSubscription.ProjectSubsciption.ProjectId,
        SubscriptionId = projectSubscription.ProjectSubsciption.SubscriptionId
      });
    }

    public StopGameServerProcessResponse StopGameServerProcess(StopGameServerProcessRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var gameServer = gameServerRepository.GetGameServerById(uw, new DataModel.GetGameServerByIdSPRequest()
        {
          GameServerId = request.GameServerId
        });

        if (gameServer == null || !gameServer.Success || gameServer.GameServer == null)
        {
          return new StopGameServerProcessResponse()
          {
            Success = false,
            Message = "Could not retrieve game server"
          };
        }

        if (gameServer.GameServer.State == GameServerState.Stopped)
        {
          return new StopGameServerProcessResponse()
          {
            Success = true,
            Message = "Game server was already in a stopped state"
          };
        }

        try
        {
          var gameServerProcess = Process.GetProcessById(gameServer.GameServer.ProcessId);
          if (gameServerProcess != null)
          {
            gameServerProcess.Kill();
          }

          gameServerRepository.SetGameServerState(uw, new SetGameServerStateSPRequest()
          {
            GameServerId = gameServer.GameServer.Id,
            State = GameServerState.Stopped
          });
        }
        catch (System.Exception e)
        {
          return new StopGameServerProcessResponse()
          {
            Success = true,
            Message = e.Message
          };
        }

        return new StopGameServerProcessResponse()
        {
          Success = true
        };
      }
    }
  }
}
