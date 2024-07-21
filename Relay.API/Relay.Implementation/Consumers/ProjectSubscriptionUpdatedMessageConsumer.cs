using MassTransit;
using Relay.Core.Factories;
using Relay.Customers.Contracts;
using Relay.DataModel;
using Relay.Repository.Interfaces;
using System.Threading.Tasks;

namespace Relay.Implementation.Consumers
{
  public class ProjectSubscriptionUpdatedMessageConsumer : IConsumer<ProjectSubscriptionUpdatedMessage>
  {
    private readonly IProjectRepository projectRepository;

    public ProjectSubscriptionUpdatedMessageConsumer(IProjectRepository projectRepository)
    {
      this.projectRepository = projectRepository;
    }

    public Task Consume(ConsumeContext<ProjectSubscriptionUpdatedMessage> context)
    {
      var msg = context.Message;

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        projectRepository.SetProjectSubscription(uw, new SetProjectSubscriptionSPRequest()
        {
          Active = msg.Active,
          ProjectId = msg.ProjectApiKey,
          SubscriptionId = msg.SubscriptionId
        });

        projectRepository.SetProjectServiceTier(uw, new SetProjectServiceTierSPRequest()
        {
          ProjectId = msg.ProjectApiKey,
          ServiceTier = msg.SubscriptionServiceCatalogId
        });
      }

      return Task.CompletedTask;
    }
  }
}
