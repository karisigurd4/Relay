namespace Relay.Implementation.Consumers
{
  using MassTransit;
  using Relay.Core.Factories;
  using Relay.Customers.Contracts;
  using Relay.Repository.Interfaces;
  using System.Threading.Tasks;

  public class ProjectCreatedMessageConsumer : IConsumer<ProjectCreatedMessage>
  {
    private readonly IProjectRepository projectRepository;

    public ProjectCreatedMessageConsumer(IProjectRepository projectRepository)
    {
      this.projectRepository = projectRepository;
    }

    public Task Consume(ConsumeContext<ProjectCreatedMessage> context)
    {
      var msg = context.Message;

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        projectRepository.CreateOrUpdateProjectSettings(uw, new DataModel.CreateOrUpdateProjectSettingsSPRequest()
        {
          ExtAuthId = msg.ExtAuthId,
          EnableMatchTimeLimit = msg.ProjectSettings.EnableMatchTimeLimit,
          EnablePreGameLobby = msg.ProjectSettings.EnablePreGameLobby,
          LobbySystemType = msg.ProjectSettings.LobbySystemType.ToString(),
          GameModesJsonData = msg.ProjectSettings.GameModesJsonData,
          MaximumActiveMatchTimeInSeconds = msg.ProjectSettings.MaximumActiveMatchTimeInSeconds,
          MaximumLobbyTimeInSeconds = msg.ProjectSettings.MaximumLobbyTimeInSeconds,
          MaximumPlayerCapacity = msg.ProjectSettings.MaximumPlayerCapacity,
          ProjectId = msg.ProjectApiKey,
          RestrictJoiningToPreGameLobby = msg.ProjectSettings.RestrictJoiningToPreGameLobby,
          EnableLevelBasedMatchmaking = msg.ProjectSettings.EnableLevelBasedMatchmaking,
          MatchmakingPlayerDataKey = msg.ProjectSettings.MatchmakingPlayerDataKey,
          MatchmakingOptimalRange = msg.ProjectSettings.MatchmakingOptimalRange
        });

        projectRepository.SetProjectSubscription(uw, new DataModel.SetProjectSubscriptionSPRequest()
        {
          Active = true,
          SubscriptionId = msg.SubscriptionId,
          ProjectId = msg.ProjectApiKey
        });
      }

      return Task.CompletedTask;
    }
  }
}
