
namespace Relay.Implementation.Infrastructure
{
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using global::AutoMapper;
  using System.Linq;

  public class AutoMapperInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(
                Classes.FromAssemblyInThisApplication(GetType().Assembly)
                .BasedOn<Profile>().WithServiceBase());

      container.Register(Component.For<IConfigurationProvider>().UsingFactoryMethod(kernel =>
      {
        return new MapperConfiguration(configuration =>
        {
          kernel.ResolveAll<Profile>().ToList().ForEach(configuration.AddProfile);
        });
      }).LifestyleSingleton());

      container.Register(
          Component.For<IMapper>().UsingFactoryMethod(kernel =>
              new Mapper(kernel.Resolve<IConfigurationProvider>(), kernel.Resolve)));
    }
  }
}
