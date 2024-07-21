using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace BitterShark.Relay
{
  [CustomEditor(typeof(RelaySceneConfiguration))]
  public class RelaySceneConfigurationEditor : Editor
  {
    private SerializedProperty additiveProperty;
    private SerializedProperty scenesProperty;
    private SerializedProperty activeSceneProperty;

    private Dictionary<string, string[]> relaySceneFoldersMap = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> sceneFoldersMap = new Dictionary<string, string[]>();

    private static int[] fieldInfoColumnWidths = new int[] { 220, 60, 60, 90, 210, 100 };



    private void OnEnable()
    {
      additiveProperty = serializedObject.FindProperty("Additive");
      scenesProperty = serializedObject.FindProperty("Scenes");
      activeSceneProperty = serializedObject.FindProperty("ActiveScene");

      relaySceneFoldersMap.Clear();
      sceneFoldersMap.Clear();

      relaySceneFoldersMap.Add("Relay/Scenes/", RelaySceneEditorUtils.GetSceneFilesInFolder("Relay\\Scenes\\"));
      sceneFoldersMap.Add("Scenes/", RelaySceneEditorUtils.GetSceneFilesInFolder("Scenes\\"));

      var relaySceneFolders = Directory.GetDirectories("Assets\\Relay\\Scenes\\").Select(x => x.Replace("Assets\\", "")).ToArray();
      var sceneFolders = Directory.GetDirectories("Assets\\Scenes\\").Select(x => x.Replace("Assets\\", "")).ToArray();

      foreach (var folder in relaySceneFolders)
      {
        relaySceneFoldersMap.Add(folder, RelaySceneEditorUtils.GetSceneFilesInFolder(folder));
      }

      foreach (var folder in sceneFolders)
      {
        sceneFoldersMap.Add(folder, RelaySceneEditorUtils.GetSceneFilesInFolder(folder));
      }
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      serializedObject.ApplyModifiedProperties();

      var sceneConfiguration = (RelaySceneConfiguration)target;

      sceneConfiguration.Scenes.RemoveAll(x => x == null);

      GUILayout.Label($"Configuration", RelayEditorUtils.HeaderFont());
      RelayEditorUtils.GuiLine();
      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      GUILayout.Label($"    Additive", GUILayout.Width(fieldInfoColumnWidths[0]));
      sceneConfiguration.Additive = EditorGUILayout.Toggle(additiveProperty.boolValue, GUILayout.Width(fieldInfoColumnWidths[1]));
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();

      GUILayout.Label($"Scenes", RelayEditorUtils.HeaderFont());
      RelayEditorUtils.GuiLine();
      EditorGUILayout.Space();

      foreach (var sceneFolder in relaySceneFoldersMap)
      {
        RelaySceneEditorUtils.DrawSceneFolderControls(activeSceneProperty, sceneConfiguration, sceneFolder);
      }

      foreach (var sceneFolder in sceneFoldersMap)
      {
        RelaySceneEditorUtils.DrawSceneFolderControls(activeSceneProperty, sceneConfiguration, sceneFolder);
      }

      EditorUtility.SetDirty(sceneConfiguration);
    }
  }
}