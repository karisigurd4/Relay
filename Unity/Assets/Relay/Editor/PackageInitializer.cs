using System.IO;
using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  [InitializeOnLoad]
  public class PackageInitializer : MonoBehaviour
  {
    static PackageInitializer()
    {
      if (ShouldOpenWindow())
      {
        RelaySettingsWindow.ShowWindow();
      }
    }

    private static bool ShouldOpenWindow()
    {
      return (!Directory.Exists("Assets/Resources/RelayPrefabs"));
    }
  }
}
