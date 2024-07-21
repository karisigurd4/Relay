using Castle.Windsor;
using Castle.Windsor.Installer;
using Relay.IntegrationTest.Interfaces;
using System;
using System.Reflection;

namespace Relay.IntegrationTest
{
  class Program
  {
    static void Main(string[] args)
    {
      IWindsorContainer container = new WindsorContainer();
      container.Install(FromAssembly.Instance(Assembly.GetCallingAssembly()));
      container.Resolve<IIntegrationTestRunner>().RunTests();
      Console.Read();
    }
  }
}
