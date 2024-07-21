using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Relay.IntegrationTest.Interfaces;

namespace Relay.IntegrationTest.Infrastructure
{
  public class TestCaseInstallers : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
      container.Register(
        Classes.FromAssemblyContaining<IIntegrationTest>().BasedOn<IIntegrationTest>().WithService.FromInterface()
      );
    }
  }
}
