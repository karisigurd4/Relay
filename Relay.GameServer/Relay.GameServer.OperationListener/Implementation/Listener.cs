namespace Relay.GameServer.OperationListener.Implementation
{
  using Relay.GameServer.Core.Factories;
  using Relay.GameServer.Core.Interfaces;
  using Relay.GameServer.Core.Utilities;
  using Relay.GameServer.DataModel;
  using Relay.GameServer.OperationListener.Interfaces;
  using Relay.GameServer.Repository.Interfaces;
  using System;
  using System.Diagnostics;
  using System.Linq;
  using System.Net;
  using System.Threading;
  using System.Threading.Tasks;

  public class Listener : IListener
  {
    private readonly IGameServerRepository gameServerRepository;
    private readonly IGameServerHostRepository gameServerHostRepository;

    private string hostName;

    public Listener(IGameServerRepository gameServerRepository, IGameServerHostRepository gameServerHostRepository)
    {
      this.gameServerRepository = gameServerRepository;
      this.gameServerHostRepository = gameServerHostRepository;

      this.hostName = Dns.GetHostName();
    }

    public void PerformActiveServersHealthCheck()
    {
      Console.WriteLine(DateTime.UtcNow.ToString() + ": Health check");

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        InactiveGameServerCleanup(uw);

        try
        {
          var memoryLines = GetWmicOutput("OS get FreePhysicalMemory,TotalVisibleMemorySize /Value").Split("\n");

          var freeMemory = memoryLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];
          var totalMemory = memoryLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];

          var cpuLines = GetWmicOutput("CPU get Name,LoadPercentage /Value").Split("\n");

          var CpuUse = cpuLines[0].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];
          var CpuName = cpuLines[1].Split("=", StringSplitOptions.RemoveEmptyEntries)[1];

          gameServerHostRepository.UpdateGameServerHostInfo(uw, new UpdateGameServerHostInfoSPRequest()
          {
            HostName = hostName,
            CpuUsage = float.Parse(CpuUse) / 100.0f,
            MemoryUsage = float.Parse(freeMemory) / float.Parse(totalMemory)
          });
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }

    private string GetWmicOutput(string query, bool redirectStandardOutput = true)
    {
      var info = new ProcessStartInfo("wmic");

      info.Arguments = query;
      info.RedirectStandardOutput = redirectStandardOutput;

      var output = string.Empty;

      using (var process = Process.Start(info))
      {
        output = process.StandardOutput.ReadToEnd();
      }

      return output.Trim();
    }

    private void InactiveGameServerCleanup(IRelayUnitOfWork uw)
    {
      var activeGameServers = gameServerRepository.GetActiveGameServers(uw, new GetActiveGameServersSPRequest()
      {
        HostName = Dns.GetHostName()
      });

      var activeProcesses = Process.GetProcesses();

      for (int i = 0; i < activeGameServers.GameServers.Length; i++)
      {
        try
        {
          var activeProcess = activeProcesses.FirstOrDefault(x => x.Id == activeGameServers.GameServers[i].ProcessId);

          if (activeProcess != null)
          {
            activeProcess.Refresh();
          }

          if (activeProcess == null || activeProcess.HasExited)
          {
            SetFinishedGameServerState(uw, activeGameServers.GameServers[i].Id);
          }
        }
        catch (Exception e)
        {
          // Exceptions can occur for a variety of reasons. 
          // The most common is that an inactive process was polled.
          // In such cases an exception is raised, marking the game server as closed when handling those seems to be a reasonable general solution.
          Console.WriteLine(DateTime.UtcNow.ToString() + ": Health check Exception");
          Console.WriteLine(e);
          Console.WriteLine(DateTime.UtcNow.ToString() + ": \nExecuting set finished state");
          SetFinishedGameServerState(uw, activeGameServers.GameServers[i].Id);
        }
      }
    }

    private void SetFinishedGameServerState(IRelayUnitOfWork uw, int gameServerId)
    {
      gameServerRepository.StopGameServer(uw, new StopGameServerSPRequest()
      {
        GameServerId = gameServerId
      });
    }

    public void Start()
    {
      Console.WriteLine(DateTime.UtcNow.ToString() + ":This application will run continuously and listen for game server executable operation requests");
      Task.Run(() =>
      {
        while (true)
        {
          PerformActiveServersHealthCheck();
          Thread.Sleep(1000);
        }
      });

      MainListenLoop();
    }

    private void MainListenLoop()
    {
      Console.WriteLine(DateTime.UtcNow.ToString() + ": Listening");
      while (true)
      {
        using (var uw = RelayUnitOfWorkFactory.Create())
        {
          var pollResponse = gameServerRepository.PollGameServerOperations(uw, new PollGameServerOperationsSPRequest()
          {
            HostName = Dns.GetHostName()
          });

          if (pollResponse.Success && pollResponse.OperationRequests.Length > 0)
          {
            Console.WriteLine(DateTime.UtcNow.ToString() + $"\nGot {pollResponse.OperationRequests.Length} operation requests.");
            for (int i = 0; i < pollResponse.OperationRequests.Length; i++)
            {
              var processStartInfo = new ProcessStartInfo()
              {
                UseShellExecute = true,
                FileName = ConfigurationReader.GetGameServerConfiguration().GameServerExecutableName,
                Arguments = $"{pollResponse.OperationRequests[i].RequestPort} {pollResponse.OperationRequests[i].RequestGameServerId} Waiting {pollResponse.OperationRequests[i].RequestProjectId}",
                WorkingDirectory = ConfigurationReader.GetGameServerConfiguration().GameServerExecutablePath
              };

              var process = Process.Start(processStartInfo);

              if (process != null)
              {
                gameServerRepository.SetGameServerProcessId(uw, new SetGameServerProcessIdSPRequest()
                {
                  ProcessId = process.Id,
                  GameServerId = pollResponse.OperationRequests[i].RequestGameServerId
                });
              }
            }
            Console.WriteLine($"");
          }
        }
        Thread.Sleep(50);
      }
    }
  }
}
