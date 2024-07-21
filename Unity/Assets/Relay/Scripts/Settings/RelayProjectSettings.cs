#if UNITY_EDITOR
#endif
using UnityEngine;

namespace BitterShark.Relay
{
  public static class RelayProjectSettings
  {
    public static string ProjectId => GetProjectSettings().ProjectId;

    public const string RelaySettingsPath = "RelayProjectSettings";

    public static RelayProjectSettingsObject GetProjectSettings()
    {
      var settings = Resources.Load<RelayProjectSettingsObject>(RelaySettingsPath);
      if (settings == null)
      {
        return null;
      }

      return Resources.Load<RelayProjectSettingsObject>("RelayProjectSettings");
    }
  }
}