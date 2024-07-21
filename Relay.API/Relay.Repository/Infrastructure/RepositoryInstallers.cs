using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Relay.Repository.Implementation;
using Relay.Repository.Interfaces;

namespace Relay.Repository.Infrastructure
{
  public class RepositoryInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
        Component.For<IGameServerRepository>()
          .ImplementedBy<GameServerRepository>()
          .LifestyleTransient(),
        Component.For<INotificationMessageRepository>()
          .ImplementedBy<NotificationMessageRepository>()
          .LifestyleTransient(),
        Component.For<IPartyRepository>()
          .ImplementedBy<PartyRepository>()
          .LifestyleTransient(),
        Component.For<IPlayerRepository>()
          .ImplementedBy<PlayerRepository>()
          .LifestyleTransient(),
        Component.For<IGameServerHostRepository>()
          .ImplementedBy<GameServerHostRepository>()
          .LifestyleTransient(),
        Component.For<IProjectRepository>()
          .ImplementedBy<ProjectRepository>()
          .LifestyleTransient()
      );
    }
  }
}
