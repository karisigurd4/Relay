using MassTransit;
using Relay.Customers.Contracts;
using System.Threading.Tasks;

namespace Relay.Implementation.Consumers
{
  public class ProjectDeletedMessageConsumer : IConsumer<ProjectSettingsUpdatedMessage>
  {
    public Task Consume(ConsumeContext<ProjectSettingsUpdatedMessage> context)
    {
      return Task.CompletedTask;
    }
  }
}
