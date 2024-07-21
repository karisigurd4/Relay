using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace BitterShark.Relay
{
  public class RelayNetworkStateWindow : EditorWindow
  {
    private Dictionary<int, bool> foldoutStates = new Dictionary<int, bool>();
    private string filterText = "";

    [MenuItem("Window/Relay/State viewer")]
    public static void ShowWindow()
    {
      EditorWindow.GetWindowWithRect(typeof(RelayNetworkStateWindow), new Rect(0, 0, 360, 650), true, "State viewer");
    }

    private void OnGUI()
    {
      if (GameServerClient.IsConnected)
      {
        var relayGameObjects = GameStateRepository.GetAll();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.TextField(filterText, GUILayout.ExpandWidth(true));

        EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        for (int i = 0; i < relayGameObjects.ToArray().Length; i++)
        {
          EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
          var r = relayGameObjects.ToArray()[i].Value;

          if (r == null)
          {
            continue;
          }

          if (!foldoutStates.ContainsKey(r.NetworkInstanceId))
          {
            foldoutStates.Add(r.NetworkInstanceId, false);
          }

          EditorGUILayout.Space();
          EditorGUILayout.BeginHorizontal();

          EditorGUILayout.BeginHorizontal(GUILayout.Width(20));
          if (GUILayout.Button(new GUIContent("", EditorGUIUtility.IconContent("TouchInputModule Icon").image), RelayEditorUtils.AlignMiddleCenter(), GUILayout.Height(15), GUILayout.Width(15)))
          {
            Selection.objects = new Object[] { r };
          }
          EditorGUILayout.EndHorizontal();

          EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          foldoutStates[r.NetworkInstanceId] = EditorGUILayout.Foldout(foldoutStates[r.NetworkInstanceId], r.name, true, RelayEditorUtils.StateFoldoutFont(foldoutStates[r.NetworkInstanceId]));
          EditorGUILayout.EndHorizontal();

          EditorGUILayout.EndHorizontal();
          EditorGUILayout.Space();

          if (foldoutStates[r.NetworkInstanceId])
          {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            GUILayout.Label($"  State Id: {relayGameObjects.ToArray()[i].Key}");
            GUILayout.Label($"  Network instance Id: {r.NetworkInstanceId}");
            GUILayout.Label($"  Relay instance Id: {r.RelayInstanceId}");
            GUILayout.Label($"  Owner Client Id: {r.OwnerClientId}");
            GUILayout.Label($"  Owner Player Id: {r.OwnerPlayerId}");
            GUILayout.Label($"  Is owner: {r.OwnerClientId == GameServerClient.ClientId}");

            EditorGUILayout.EndVertical();
          }
          EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
      }
    }
  }
}