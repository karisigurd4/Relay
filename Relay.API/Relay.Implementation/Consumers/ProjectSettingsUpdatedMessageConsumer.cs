namespace Relay.Implementation.Consumers
{
  using MassTransit;
  using Relay.Core.Factories;
  using Relay.Customers.Contracts;
  using Relay.Repository.Interfaces;
  using System.Threading.Tasks;

  public class ProjectSettingsUpdatedMessageConsumer : IConsumer<ProjectSettingsUpdatedMessage>
  {
    private readonly IProjectRepository projectRepository;

    public ProjectSettingsUpdatedMessageConsumer(IProjectRepository projectRepository)
    {
      this.projectRepository = projectRepository;
    }

    public Task Consume(ConsumeContext<ProjectSettingsUpdatedMessage> context)
    {
      var msg = context.Message;

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        projectRepository.CreateOrUpdateProjectSettings(uw, new DataModel.CreateOrUpdateProjectSettingsSPRequest()
        {
          ExtAuthId = msg.ExtAuthId,
          ServiceCatalogId = msg.ProjectSettings.ServiceCatalogId,
          LobbySystemType = msg.ProjectSettings.LobbySystemType.ToString(),
          GameModesJsonData = msg.ProjectSettings.GameModesJsonData,
          EnableMatchTimeLimit = msg.ProjectSettings.EnableMatchTimeLimit,
          EnablePreGameLobby = msg.ProjectSettings.EnablePreGameLobby,
          MaximumActiveMatchTimeInSeconds = msg.ProjectSettings.MaximumActiveMatchTimeInSeconds,
          MaximumLobbyTimeInSeconds = msg.ProjectSettings.MaximumLobbyTimeInSeconds,
          MaximumPlayerCapacity = msg.ProjectSettings.MaximumPlayerCapacity,
          ProjectId = msg.ProjectApiKey,
          RestrictJoiningToPreGameLobby = msg.ProjectSettings.RestrictJoiningToPreGameLobby,
          EnableLevelBasedMatchmaking = msg.ProjectSettings.EnableLevelBasedMatchmaking,
          MatchmakingPlayerDataKey = msg.ProjectSettings.MatchmakingPlayerDataKey,
          MatchmakingOptimalRange = msg.ProjectSettings.MatchmakingOptimalRange
        });
      }

      return Task.CompletedTask;
    }
  }
}
