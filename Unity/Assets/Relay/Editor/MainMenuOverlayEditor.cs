using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  [CustomEditor(typeof(MainMenuOverlay))]
  public class MainMenuOverlayEditor : Editor
  {
    private Dictionary<string, string[]> sceneFoldersMap = new Dictionary<string, string[]>();
    private static int[] matchmakingFieldColumnWidths = new int[] { 220, 60, 90, 90, 210, 100 };
    private static int[] gameServerBrowserFieldColumnWidths = new int[] { 140, 90, 30, 90, 210, 100 };
    private static int[] fieldInfoColumnWidths = new int[] { 100, 100, 30 };

    private SerializedProperty gameModesProperty;

    private int addGameModeSceneIndex = 0;
    private string addGameModeMaxPlayers = "16";
    private string singleGameModeMaxPlayers = "16";
    private string addGameModeName = "";
    private bool showGameModes = false;

    private void OnEnable()
    {
      gameModesProperty = serializedObject.FindProperty("MultipleModesSceneConfigurations");

      sceneFoldersMap.Clear();
      sceneFoldersMap.Add("Scenes/", RelaySceneEditorUtils.GetSceneFilesInFolder("Scenes/"));
      var sceneFolders = Directory.GetDirectories("Assets/Scenes/").Select(x => x.Replace("Assets/", "")).ToArray();
      foreach (var folder in sceneFolders)
      {
        sceneFoldersMap.Add(folder, RelaySceneEditorUtils.GetSceneFilesInFolder(folder));
      }
    }

    public override void OnInspectorGUI()
    {
      var mainMenuOverlay = (MainMenuOverlay)target;

      EditorGUILayout.BeginVertical();

      EditorGUILayout.Space();
      GUILayout.Label($"General configuration", RelayEditorUtils.HeaderFont());
      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();

      GUILayout.Label($"Lobby system type: ", GUILayout.Width(gameServerBrowserFieldColumnWidths[0]));
      var newLobbySystemType = (LobbySystemType)EditorGUILayout.Popup((int)mainMenuOverlay.LobbySystemType, Enum.GetNames(typeof(LobbySystemType)));
      if (newLobbySystemType != mainMenuOverlay.LobbySystemType)
      {
        mainMenuOverlay.LobbySystemType = newLobbySystemType;
        EditorUtility.SetDirty(mainMenuOverlay);
      }

      EditorGUILayout.EndHorizontal();

      EditorGUILayout.Space();
      GUILayout.Label($"Configuration", RelayEditorUtils.HeaderFont());
      EditorGUILayout.Space();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Multiple game modes", "Enable if your game has multiple selectable maps or game modes."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(gameServerBrowserFieldColumnWidths[0]));
      //var newValue = EditorGUILayout.Toggle(mainMenuOverlay.AllowMultipleModes, GUILayout.Width(gameServerBrowserFieldColumnWidths[1]));
      //if (newValue && !mainMenuOverlay.AllowMultipleModes)
      //{
      //  mainMenuOverlay.AllowMultipleModes = true;
      //  EditorUtility.SetDirty(mainMenuOverlay);
      //}
      //else if (!newValue && mainMenuOverlay.AllowMultipleModes)
      //{
      //  mainMenuOverlay.AllowMultipleModes = false;
      //  EditorUtility.SetDirty(mainMenuOverlay);
      //}
      //EditorGUILayout.EndHorizontal();

      if (!mainMenuOverlay.AllowMultipleModes)
      {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Max players"), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(gameServerBrowserFieldColumnWidths[0]));
        var newSingleGameModeMaxPlayers = GUILayout.TextField(singleGameModeMaxPlayers, GUILayout.ExpandWidth(true));
        if (singleGameModeMaxPlayers != mainMenuOverlay.SingleGameModeMaxPlayers.ToString())
        {
          int newMaxPlayers = 0;
          if (int.TryParse(newSingleGameModeMaxPlayers, out newMaxPlayers))
          {
            mainMenuOverlay.SingleGameModeMaxPlayers = newMaxPlayers;
            EditorUtility.SetDirty(mainMenuOverlay);
          }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Scene", "Select the in-game scene."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(gameServerBrowserFieldColumnWidths[0]));
        EditorGUILayout.Popup((int)mainMenuOverlay.LobbySystemType, GetSceneSelection());
        EditorGUILayout.EndHorizontal();

      }
      else
      {

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Name"), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0]));
        EditorGUILayout.LabelField(new GUIContent("Scene"), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[1]));
        EditorGUILayout.LabelField(new GUIContent("Max players"), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[2]));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical("");
        for (int i = 0; i < mainMenuOverlay.GameModeConfigurations.Count; i++)
        {
          var gameModeEntry = mainMenuOverlay.GameModeConfigurations[i];

          EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
          EditorGUILayout.LabelField(new GUIContent(RelayEditorUtils.TruncateWithEllipsis(gameModeEntry.ModeName, 12)), RelayEditorUtils.FieldNameFont(true, TextAnchor.MiddleLeft, 11), GUILayout.Width(fieldInfoColumnWidths[0]));
          EditorGUILayout.LabelField(new GUIContent(Path.GetFileName(gameModeEntry.SceneName)), RelayEditorUtils.FieldNameFont(true, TextAnchor.MiddleLeft, 11), GUILayout.Width(fieldInfoColumnWidths[1]));
          EditorGUILayout.LabelField(new GUIContent(gameModeEntry.MaxPossiblePlayers.ToString()), RelayEditorUtils.FieldNameFont(true, TextAnchor.MiddleLeft, 11), GUILayout.Width(fieldInfoColumnWidths[2]));
          if (GUILayout.Button("X", RelayEditorUtils.BoldRedFont(TextAnchor.MiddleLeft)))
          {
            mainMenuOverlay.GameModeConfigurations.RemoveAt(i);
            EditorUtility.SetDirty(mainMenuOverlay);
          }
          EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent($"Total: {mainMenuOverlay.GameModeConfigurations.Count}"), RelayEditorUtils.SmallInfoFont(), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Name"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
        addGameModeName = GUILayout.TextField(addGameModeName, GUILayout.Width(fieldInfoColumnWidths[1]), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Max players"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
        addGameModeMaxPlayers = GUILayout.TextField(addGameModeMaxPlayers, GUILayout.Width(fieldInfoColumnWidths[1]), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Scene"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
        var selectedAddGameModeSceneIndex = EditorGUILayout.Popup(addGameModeSceneIndex, GetSceneSelection(), GUILayout.ExpandWidth(true));
        if (selectedAddGameModeSceneIndex != addGameModeSceneIndex)
        {
          addGameModeSceneIndex = selectedAddGameModeSceneIndex;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent(""), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
        if (GUILayout.Button("Add", GUILayout.Width(50)))
        {
          mainMenuOverlay.GameModeConfigurations.Add(new RelayGameModeConfiguration()
          {
            MaxPossiblePlayers = int.Parse(addGameModeMaxPlayers),
            ModeName = addGameModeName,
            SceneName = GetSceneSelection()[addGameModeSceneIndex]
          });
          EditorUtility.SetDirty(mainMenuOverlay);

          addGameModeSceneIndex = 0;
          addGameModeName = "";
          addGameModeMaxPlayers = "16";
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();

      EditorUtility.SetDirty(mainMenuOverlay);
      serializedObject.Update();
      serializedObject.ApplyModifiedProperties();
    }

    public string[] GetSceneSelection()
    {
      return sceneFoldersMap.SelectMany(folders => folders.Value.Select(scene => $"Assets/{folders.Key + scene}.unity")).ToArray();
    }
  }
}