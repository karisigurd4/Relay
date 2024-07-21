namespace Relay.GameServer.App
{
  using Castle.Windsor;
  using Castle.Windsor.Installer;
  using Relay.GameServer.Core.Types;
  using Relay.GameServer.Implementation.Interfaces;
  using System;
  using System.Reflection;

  public class Program
  {
    public static void Main(string[] args)
    {
      if (args == null || args.Length < 2)
      {
        Console.WriteLine($"Usage: ---.exe {{PORT}} {{ID}} (optional) {{STATE}} {{PROJECTID}}");
        return;
      }

      ushort port = 0;
      if (!ushort.TryParse(args[0], out port))
      {
        Console.WriteLine($"Could not parse first {{PORT}} parameter. Must be a valid USHORT.");
        return;
      }

      int gameServerId = 0;
      if (!int.TryParse(args[1], out gameServerId))
      {
        Console.WriteLine($"Could not parse second {{ID}} parameter. Must be a valid INT.");
        return;
      }

      GameServerState gameServerStartState = GameServerState.Waiting;
      if (args.Length > 2)
      {
        try
        {
          gameServerStartState = (GameServerState)Enum.Parse(typeof(GameServerState), args[2]);
        }
        catch
        {
          Console.WriteLine($"Could not parse third {{STATE}} parameter.");
        }
      }

      Guid projectId = Guid.Empty;
      if (args.Length > 3)
      {
        if (!Guid.TryParse(args[3], out projectId))
        {
          Console.WriteLine($"Could not parse fourth {{PROJECTID}} parameter.");
        }
      }

      IWindsorContainer container = new WindsorContainer();
      container.Install(FromAssembly.Instance(Assembly.GetCallingAssembly()));
      container.Install(FromAssembly.Named("Relay.GameServer.Implementation"));
      container.Install(FromAssembly.Named("Relay.GameServer.Repository"));

      var gameServer = container.Resolve<IGameServer>();

      gameServer.Start(gameServerId, port, gameServerStartState, projectId);
    }
  }
}
