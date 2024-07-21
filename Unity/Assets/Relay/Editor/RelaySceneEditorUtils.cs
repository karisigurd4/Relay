using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BitterShark.Relay
{
  public static class RelaySceneEditorUtils
  {
    private static int[] fieldInfoColumnWidths = new int[] { 220, 60, 60, 90, 210, 100 };

    public static string[] GetSceneFilesInFolder(string folder)
    {
      return Directory.GetFiles("Assets/" + folder + '/')
            .Select(x => x.Split('/').LastOrDefault() ?? "")
            .Where(x => x.EndsWith(".unity"))
            .Select(x => x.Split('.')[0])
            .ToArray();
    }

    public static void DrawSceneFolderControls(SerializedProperty activeSceneProperty, RelaySceneConfiguration sceneConfiguration, KeyValuePair<string, string[]> sceneFolder)
    {
      EditorGUILayout.BeginVertical();

      EditorGUILayout.LabelField(new GUIContent($"{sceneFolder.Key}"), RelayEditorUtils.HeaderRowFont());

      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("    Name"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      EditorGUILayout.LabelField(new GUIContent("Include"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      EditorGUILayout.LabelField(new GUIContent("Active"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[2]));
      EditorGUILayout.LabelField(new GUIContent("Scene type"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[3]));
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();

      foreach (var scene in sceneFolder.Value)
      {
        var sceneName = "Assets/" + sceneFolder.Key + scene + ".unity";

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label($"        {scene}", GUILayout.Width(fieldInfoColumnWidths[0]));

        var includedScenes = new List<string>();
        for (int i = 0; i < sceneConfiguration.Scenes.Count; i++)
        {
          includedScenes.Add(sceneConfiguration.Scenes[i].Name);
        }

        var toggleSceneValue = EditorGUILayout.Toggle(includedScenes.Contains(sceneName), GUILayout.Width(fieldInfoColumnWidths[1]));
        if (toggleSceneValue)
        {
          if (!sceneConfiguration.Scenes.Any(x => x.Name == sceneName))
          {
            sceneConfiguration.Scenes.Add(new RelayScene() { Name = sceneName });
          }
        }
        else
        {
          if (sceneConfiguration.Scenes.Any(x => x.Name == sceneName))
          {
            sceneConfiguration.Scenes.Remove(sceneConfiguration.Scenes.FirstOrDefault(x => x.Name == sceneName));
          }
        }

        var toggleActiveSceneValue = EditorGUILayout.Toggle(activeSceneProperty.stringValue == sceneName, GUILayout.Width(fieldInfoColumnWidths[2]));
        if (toggleActiveSceneValue)
        {
          if (sceneConfiguration.ActiveScene != sceneName)
          {
            sceneConfiguration.ActiveScene = sceneName;
          }
        }
        else
        {
          if (sceneConfiguration.ActiveScene == sceneName)
          {
            sceneConfiguration.ActiveScene = string.Empty;
          }
        }

        if (sceneConfiguration.Scenes.FirstOrDefault(x => x.Name == sceneName) != null)
        {
          sceneConfiguration.Scenes.FirstOrDefault(x => x.Name == sceneName).SceneType = (RelaySceneType)EditorGUILayout.Popup((int)sceneConfiguration.Scenes.FirstOrDefault(x => x.Name == sceneName).SceneType, Enum.GetNames(typeof(RelaySceneType)), GUILayout.Width(fieldInfoColumnWidths[3]));
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        if (sceneConfiguration.Scenes.FirstOrDefault(x => x.Name == sceneName) != null)
        {
          EditorGUILayout.BeginHorizontal();

          if (SceneUtility.GetBuildIndexByScenePath(sceneName.Replace("/", "/")) == -1)
          {
            GUILayout.Label($"    Scene needs to be included in build", RelayEditorUtils.RequiredFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
            if (GUILayout.Button("Add", GUILayout.Width(fieldInfoColumnWidths[1])))
            {
              var buildSettingsScenes = EditorBuildSettings.scenes.ToList();
              buildSettingsScenes.Add(new EditorBuildSettingsScene("Assets/" + sceneFolder.Key + "/" + scene + ".unity", true));
              EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
              EditorGUILayout.Space();
            }
          }
          EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.Space();
    }
  }
}