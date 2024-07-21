namespace Relay.GameServer.OperationListener
{
  using Castle.Windsor;
  using Castle.Windsor.Installer;
  using Relay.GameServer.OperationListener.Interfaces;
  using System.Reflection;

  public class Program
  {
    public static void Main(string[] args)
    {
      IWindsorContainer container = new WindsorContainer();
      container.Install(FromAssembly.Instance(Assembly.GetCallingAssembly()));
      container.Install(FromAssembly.Named("Relay.GameServer.Repository"));

      var listener = container.Resolve<IListener>();

      listener.Start();
    }
  }
}