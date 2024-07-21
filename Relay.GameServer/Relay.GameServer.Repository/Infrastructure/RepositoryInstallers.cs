namespace Relay.GameServer.Repository.Infrastructure
{
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Relay.GameServer.Repository.Implementation;
  using Relay.GameServer.Repository.Interfaces;

  public class RepositoryInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
        Component.For<IGameObjectStateRepository>()
          .ImplementedBy<GameObjectStateRepository>()
          .LifestyleSingleton(),
        Component.For<IGameServerRepository>()
          .ImplementedBy<GameServerRepository>()
          .LifestyleTransient(),
        Component.For<IServiceCatalogConfigurationRepository>()
          .ImplementedBy<ServiceCatalogConfigurationRepository>()
          .LifestyleTransient(),
        Component.For<IGameServerHostRepository>()
          .ImplementedBy<GameServerHostRepository>()
          .LifestyleSingleton()
      );
    }
  }
}
