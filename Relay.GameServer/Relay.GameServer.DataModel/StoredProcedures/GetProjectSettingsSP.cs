namespace Relay.GameServer.DataModel
{
  using System;

  public class GetProjectSettingsSPRequest
  {
    public Guid ProjectId { get; set; }
  }

  public class GetProjectSettingsSPResponseJson
  {
    public bool Success { get; set; }
    public string ProjectSettingsJson { get; set; }
  }

  public class GetProjectSettingsSPResponse
  {
    public bool Success { get; set; }
    public ProjectSettings ProjectSettings { get; set; }
  }

  public static class GetProjectSettingsSP
  {
    public static string Name => "[Relay].[GetProjectSettings]";

    public static object CreateParameters(Guid projectId) => new
    {
      projectId
    };
  }
}
