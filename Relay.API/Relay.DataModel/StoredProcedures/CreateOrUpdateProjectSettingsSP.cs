namespace Relay.DataModel
{
  using System;

  public class CreateOrUpdateProjectSettingsSPResponse
  {
    public bool Success { get; set; }
  }

  public class CreateOrUpdateProjectSettingsSPRequest
  {
    public string ExtAuthId { get; set; }
    public Guid ProjectId { get; set; }
    public int ServiceCatalogId { get; set; }
    public string GameModesJsonData { get; set; }
    public string LobbySystemType { get; set; }
    public int MaximumPlayerCapacity { get; set; }
    public bool EnablePreGameLobby { get; set; }
    public bool RestrictJoiningToPreGameLobby { get; set; }
    public int MaximumLobbyTimeInSeconds { get; set; }
    public bool EnableMatchTimeLimit { get; set; }
    public int MaximumActiveMatchTimeInSeconds { get; set; }
    public bool EnableLevelBasedMatchmaking { get; set; }
    public string MatchmakingPlayerDataKey { get; set; }
    public int MatchmakingOptimalRange { get; set; }
  }

  public static class CreateOrUpdateProjectSettingsSP
  {
    public static string Name => "[Relay].[CreateOrUpdateProjectSettings]";

    public static object CreateParameters
    (
      string extAuthId,
      Guid projectId,
      int serviceCatalogId,
      string lobbySystemType,
      string gameModesJsonData,
      int maximumPlayerCapacity,
      bool enablePreGameLobby,
      bool restrictJoiningToPreGameLobby,
      int maximumLobbyTimeInSeconds,
      bool enableMatchTimeLimit,
      int maximumActiveMatchTimeInSeconds,
      bool enableLevelBasedMatchmaking,
      string matchmakingPlayerDataKey,
      int matchmakingOptimalRange
    ) => new
    {
      extAuthId,
      projectId,
      serviceCatalogId,
      lobbySystemType,
      gameModesJsonData,
      maximumPlayerCapacity,
      enablePreGameLobby,
      restrictJoiningToPreGameLobby,
      maximumLobbyTimeInSeconds,
      enableMatchTimeLimit,
      maximumActiveMatchTimeInSeconds,
      enableLevelBasedMatchmaking,
      matchmakingPlayerDataKey,
      matchmakingOptimalRange
    };
  }
}
