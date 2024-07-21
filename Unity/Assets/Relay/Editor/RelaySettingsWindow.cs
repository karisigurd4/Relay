using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BitterShark.Relay
{
  public class RelaySettingsWindow : EditorWindow
  {
    private static Texture logoTexture;

    private static bool saved;
    private static string ProjectId;
    public const string RelaySettingsPath = "Assets/Resources/RelayProjectSettings.asset";

    [MenuItem("Window/Relay/Startup")]
    public static void ShowWindow()
    {
      var settings = AssetDatabase.LoadAssetAtPath<RelayProjectSettingsObject>(RelaySettingsPath);
      if (settings != null)
      {
        ProjectId = settings.ProjectId;
      }
      saved = false;
      EditorWindow.GetWindowWithRect(typeof(RelaySettingsWindow), new Rect(0, 0, 550, 300), false, "Relay startup");
    }

    private void AddSceneToBuild(string scene)
    {
      if (SceneUtility.GetBuildIndexByScenePath(scene) == -1)
      {
        var buildSettingsScenes = EditorBuildSettings.scenes.ToList();
        buildSettingsScenes.Add(new EditorBuildSettingsScene(scene, true));
        EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
      }
    }

    private void OnGUI()
    {
      AddSceneToBuild("Assets/Relay/Scenes/LobbyUI.unity");
      AddSceneToBuild("Assets/Relay/Scenes/PostGameUI.unity");
      AddSceneToBuild("Assets/Relay/Scenes/InGameDebugOverlay.unity");
      AddSceneToBuild("Assets/Relay/Scenes/MainMenuOverlay.unity");
      AddSceneToBuild("Assets/Relay/Scenes/TransitionIn.unity");
      AddSceneToBuild("Assets/Relay/Scenes/TransitionOut.unity");
      AddSceneToBuild("Assets/Relay/Scenes/Loading.unity");
      AddSceneToBuild("Assets/Relay/Scenes/PostGame.unity");

      EditorGUILayout.Space();
      EditorGUILayout.Space();
      EditorGUILayout.Space();

      EditorGUILayout.BeginVertical();

      EditorGUILayout.LabelField(new GUIContent("Getting Started"), RelayEditorUtils.CenterHeaderFont());
      EditorGUI.indentLevel++;
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUILayout.LabelField(new GUIContent("Make sure to sign up for a free account in order to register your project and retrieve your Project API key."), RelayEditorUtils.CenterLabelFont(), GUILayout.MaxWidth(450));
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      EditorGUI.indentLevel--;

      EditorGUILayout.Space();

      if (AddButton("Sign up for an Account"))
      {
        Application.OpenURL("https://www.bittershark.com");
      }

      if (AddButton("Documentation"))
      {
        Application.OpenURL("https://www.bittershark.com/#/documentation");
      }

      EditorGUILayout.Space();
      EditorGUILayout.Space();

      RelayEditorUtils.GuiLine();

      EditorGUILayout.LabelField(new GUIContent("Project Configuration"), RelayEditorUtils.CenterHeaderFont());
      EditorGUI.indentLevel++;
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUILayout.LabelField(new GUIContent("Register your project in your account dashboard, click the 'View Api Key' button and copy/paste your project's API key into the input box below and click save."), RelayEditorUtils.CenterLabelFont(), GUILayout.MaxWidth(450));
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      EditorGUI.indentLevel--;

      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUILayout.LabelField(new GUIContent("Your project API key"), RelayEditorUtils.CenterHeaderFont());
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      ProjectId = EditorGUILayout.TextField(ProjectId, GUILayout.Width(270));
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();

      if (!Directory.Exists("Assets/Resources"))
      {
        Directory.CreateDirectory("Assets/Resources");
      }

      if (!Directory.Exists("Assets/Resources/RelayPrefabs"))
      {
        Directory.CreateDirectory("Assets/Resources/RelayPrefabs");
        File.Copy("Assets/Relay/Prefabs/GameServerConnection/PlayerBase.prefab", "Assets/Resources/RelayPrefabs/PlayerBase.prefab");
        AssetDatabase.Refresh();
      }

      if (AddButton("Save"))
      {
        var settings = AssetDatabase.LoadAssetAtPath<RelayProjectSettingsObject>(RelaySettingsPath);
        if (settings == null)
        {
          settings = ScriptableObject.CreateInstance<RelayProjectSettingsObject>();
          settings.ProjectId = ProjectId;
          AssetDatabase.CreateAsset(settings, RelaySettingsPath);
          EditorUtility.SetDirty(settings);
          AssetDatabase.SaveAssets();
        }
        else
        {
          settings.ProjectId = ProjectId;
          EditorUtility.SetDirty(settings);
          AssetDatabase.SaveAssets();
        }
        saved = true;
      }

      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (saved)
      {
        GUILayout.Label("Saved!");
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();
      EditorGUILayout.Space();

      EditorGUILayout.Space();


      EditorGUILayout.EndVertical();
    }

    private bool AddButton(string text)
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      var value = GUILayout.Button(text, GUILayout.Width(200));
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      return value;
    }
  }
}