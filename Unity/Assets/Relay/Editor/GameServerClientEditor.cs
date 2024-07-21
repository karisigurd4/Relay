using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  [CustomEditor(typeof(GameServerClient))]
  public class GameServerClientEditor : Editor
  {
    private static int[] fieldInfoColumnWidths = new int[] { 160, 60, 80 };

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      serializedObject.ApplyModifiedProperties();

      var client = (GameServerClient)target;

      GUILayout.Label($"    Information", RelayEditorUtils.HeaderFont());
      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        Connect to local:", "Used for testing."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      var newValue = EditorGUILayout.Toggle(client.ConnectToLocal);
      if (newValue && !client.ConnectToLocal)
      {
        client.ConnectToLocal = true;
      }
      else if (!newValue && client.ConnectToLocal)
      {
        client.ConnectToLocal = false;
      }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        Test inactivity:", "Used for testing."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      var newInactivityValue = EditorGUILayout.Toggle(client.TestInactivity);
      if (newInactivityValue && !client.TestInactivity)
      {
        client.TestInactivity = true;
      }
      else if (!newInactivityValue && client.TestInactivity)
      {
        client.TestInactivity = false;
      }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        ClientId:", "Client Id assigned by the game server."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      EditorGUILayout.LabelField(new GUIContent((client != null && GameServerClient.IsConnected ? GameServerClient.ClientId : 0).ToString(), "Client Id assigned by the game server."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[1]));
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        Is lowest Client Id:", "Whether the assigned client id is lowest of all connected clients."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      EditorGUILayout.LabelField(new GUIContent((client != null && GameServerClient.IsConnected ? GameServerClient.IsLowestIdClient().ToString() : "N/A"), "Whether the assigned client id is lowest of all connected clients."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[1]));
      EditorGUILayout.EndHorizontal();

      EditorUtility.SetDirty(client);
    }
  }
}
