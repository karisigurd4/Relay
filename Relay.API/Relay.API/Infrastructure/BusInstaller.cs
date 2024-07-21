using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using Relay.Core.Utilities;
using Relay.Implementation.Consumers;

namespace Relay.API.Infrastructure
{
  public class BusInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      /*			

      Not used in this release. Was used for CMS integration. Access key has been removed.    

      container.AddMassTransit(x =>
      {
        x.AddConsumer<ProjectCreatedMessageConsumer>();
        x.AddConsumer<ProjectSettingsUpdatedMessageConsumer>();
        x.AddConsumer<ProjectSubscriptionUpdatedMessageConsumer>();

        x.AddBus(context => Bus.Factory.CreateUsingAzureServiceBus(config =>
        {
          config.Host("Endpoint=sb://relay-servicebus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=");
          config.UseServiceBusMessageScheduler();

          config.ReceiveEndpoint(ConfigurationReader.GetEnvironmentConfiguration().ServiceName + "_queue", endpoint =>
          {
            endpoint.UseMessageScope();
            endpoint.UseServiceBusMessageScheduler();

            endpoint.ConfigureConsumer<ProjectCreatedMessageConsumer>(context);
            endpoint.ConfigureConsumer<ProjectSubscriptionUpdatedMessageConsumer>(context);
            endpoint.ConfigureConsumer<ProjectSettingsUpdatedMessageConsumer>(context);
          });

          config.ConfigureEndpoints(context);
        }));
      });
      */
      //var busControl = container.Resolve<IBusControl>();
      //busControl.Start();
    }
  }
}
