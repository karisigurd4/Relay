namespace Relay.Implementation.Infrastructure
{
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Relay.Implementation.Implementation;
  using Relay.Implementation.Interfaces;

  public class ImplementationInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
        Component.For<IGameServerManager>()
          .ImplementedBy<GameServerManager>()
          .LifestyleTransient(),
        Component.For<IPlayerManager>()
          .ImplementedBy<PlayerManager>()
          .LifestyleTransient(),
        Component.For<INotificationMessageManager>()
          .ImplementedBy<NotificationMessageManager>()
          .LifestyleTransient(),
        Component.For<IPartyManager>()
          .ImplementedBy<PartyManager>()
          .LifestyleTransient(),
        Component.For<IGameServerProcessManager>()
          .ImplementedBy<GameServerProcessManager>()
          .LifestyleSingleton(),
        Component.For<IGameServerHostManager>()
          .ImplementedBy<GameServerHostManager>()
          .LifestyleTransient()
      );
    }
  }
}
