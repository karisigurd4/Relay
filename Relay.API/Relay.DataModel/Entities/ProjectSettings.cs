namespace Relay.DataModel
{
  public class ProjectSettings
  {
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
}
