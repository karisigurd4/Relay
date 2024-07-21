namespace Relay.GameServer.Implementation.Infrastructure
{
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Relay.GameServer.Implementation.Implementation;
  using Relay.GameServer.Implementation.Interfaces;

  public class ImplementationInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
        Component.For<IGameServer>()
          .ImplementedBy<GameServer>()
          .LifestyleSingleton(),
        Component.For<IClientConnectionManager>()
          .ImplementedBy<ClientConnectionManager>()
          .LifestyleSingleton(),
        Component.For<IGameServerStateManager>()
          .ImplementedBy<GameServerStateManager>()
          .LifestyleSingleton()
      );
    }
  }
}
