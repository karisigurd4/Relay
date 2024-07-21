using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Relay.IntegrationTest.Implementation;
using Relay.IntegrationTest.Interfaces;

namespace Relay.IntegrationTest.Infrastructure
{
  public class ImplementationInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
        Component.For<IIntegrationTestRunner>()
          .ImplementedBy<IntegrationTestRunner>()
          .LifestyleSingleton()
      );
    }
  }
}
