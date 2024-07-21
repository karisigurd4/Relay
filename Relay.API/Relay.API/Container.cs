using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Reflection;

namespace Relay.API
{
  public static class Container
  {
    public static IWindsorContainer WindsorContainer;

    public static void Install()
    {
      WindsorContainer = new WindsorContainer();
      WindsorContainer.Install(FromAssembly.Instance(Assembly.GetCallingAssembly()));
      WindsorContainer.Install(FromAssembly.Named("Relay.Implementation"));
      WindsorContainer.Install(FromAssembly.Named("Relay.Repository"));
    }
  }
}
