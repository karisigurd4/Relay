namespace Relay.GameServer.OperationListener.Infrastructure
{
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Relay.GameServer.OperationListener.Implementation;
  using Relay.GameServer.OperationListener.Interfaces;

  public class ImplementationInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Component.For<IListener>().ImplementedBy<Listener>().LifestyleSingleton());
    }
  }
}
