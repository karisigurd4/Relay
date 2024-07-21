using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  [CustomEditor(typeof(RelayGameObjectManager))]
  public class RelayGameObjectManagerEditor : Editor
  {
    private static int[] fieldInfoColumnWidths = new int[] { 160, 60, 80 };

    public override void OnInspectorGUI()
    {
      var objectManager = (RelayGameObjectManager)target;

      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
      EditorGUILayout.LabelField(new GUIContent("Name"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      EditorGUILayout.LabelField(new GUIContent("Instance Id"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();

      for (int i = 0; i < objectManager.relayGameObjectsCache.Count; i++)
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent(objectManager.relayGameObjectsCache.ElementAt(i).Value.name), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
        EditorGUILayout.LabelField(new GUIContent(objectManager.relayGameObjectsCache.ElementAt(i).Key.ToString()), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
        EditorGUILayout.EndHorizontal();
      }
    }
  }
}