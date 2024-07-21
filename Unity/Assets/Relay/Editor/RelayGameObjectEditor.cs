using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  [CustomEditor(typeof(RelayGameObject))]
  public class RelayGameObjectEditor : Editor
  {
    private bool showBuiltInReplicationOptions = true;

    private SerializedProperty onInitializeLocalTrue;
    private SerializedProperty onInitializeLocalFalse;

    private Dictionary<string, bool> showComponentReplicationOptions = new Dictionary<string, bool>();
    private Dictionary<string, int> fieldReplicationUpdateMethod = new Dictionary<string, int>();

    private static int[] fieldInfoColumnWidths = new int[] { 120, 60, 55 };

    private void OnEnable()
    {
      onInitializeLocalTrue = serializedObject.FindProperty("OnInitializeLocalTrue");
      onInitializeLocalFalse = serializedObject.FindProperty("OnInitializeLocalFalse");
    }

    private static Type[] UnityPrimitives = new Type[] {
    typeof(Vector2),
    typeof(Vector3),
    typeof(Vector4),
    typeof(Quaternion)
  };

    public override void OnInspectorGUI()
    {
      EditorGUILayout.Space();

      var relayGameObject = (RelayGameObject)target;

      EditorGUILayout.BeginVertical("toolbarDropDown");
      if (GUILayout.Button("Relay managed", RelayEditorUtils.FoldoutFont(showBuiltInReplicationOptions)))
      {
        showBuiltInReplicationOptions = !showBuiltInReplicationOptions;
      }
      EditorGUILayout.EndVertical();

      RelayEditorUtils.BottonPad(2);

      if (showBuiltInReplicationOptions)
      {
        DrawBuiltInStateReplicationOptions(relayGameObject);
      }

      var gameObjectComponents = relayGameObject.GetComponents(typeof(Component));
      for (int i = 0; i < gameObjectComponents.Length; i++)
      {
        if (gameObjectComponents[i].GetType() == typeof(RelayGameObject))
        {
          continue;
        }

        //if (gameObjectComponents[i].GetType().GetCustomAttributes(typeof(RelaySerializableAttribute), true).Length == 0)
        //{
        //  continue;
        //}

        var componentKey = gameObjectComponents[i].GetType().Name;

        const BindingFlags fieldbindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField;
        const BindingFlags propertybindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;
        var fields = gameObjectComponents[i].GetType().GetFields(fieldbindingFlags).Cast<MemberInfo>()
          .Concat
          (
            gameObjectComponents[i].GetType().GetProperties(propertybindingFlags)
              .Where(x => x.SetMethod != null)
              .Cast<MemberInfo>()
          )
          .ToArray();

        if (fields.Length > 0)
        {
          if (!showComponentReplicationOptions.ContainsKey(componentKey))
          {
            showComponentReplicationOptions.Add(componentKey, false);
          }

          EditorGUILayout.Space();

          EditorGUILayout.BeginHorizontal("toolbarDropDown");
          showComponentReplicationOptions[gameObjectComponents[i].GetType().Name] = EditorGUILayout.Foldout(showComponentReplicationOptions[componentKey], $"    {componentKey}", true, RelayEditorUtils.FoldoutFont(showComponentReplicationOptions[componentKey]));
          EditorGUILayout.EndHorizontal();

          if (showComponentReplicationOptions[componentKey])
          {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(new GUIContent("    Field"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
            EditorGUILayout.LabelField(new GUIContent("Replicate"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
            EditorGUILayout.LabelField(new GUIContent("Type info"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[2]));
            EditorGUILayout.LabelField(new GUIContent("Update method"), RelayEditorUtils.HeaderRowFont(), GUILayout.Width(fieldInfoColumnWidths[2]));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            for (int x = 0; x < fields.Length; x++)
            {
              var type = (fields[x].MemberType == MemberTypes.Property ? ((PropertyInfo)fields[x]).PropertyType : ((FieldInfo)fields[x]).FieldType);

              if (!type.IsPrimitive && !UnityPrimitives.Contains(type))
              {
                continue;
              }

              EditorGUILayout.BeginHorizontal();
              var fieldKey = componentKey + fields[x].Name;

              var selected = relayGameObject.FieldConfigurations.Any(f => f.ComponentName == componentKey && f.FieldName == fields[x].Name);

              EditorGUILayout.LabelField(new GUIContent("    " + fields[x].Name, "Replicate the value of this field across the network."), RelayEditorUtils.FieldNameFont(selected), GUILayout.Width(fieldInfoColumnWidths[0]));

              var fieldType = type;
              var newFieldValue = EditorGUILayout.Toggle(selected, GUILayout.Width(fieldInfoColumnWidths[1]));
              if (newFieldValue)
              {
                if (!selected)
                {
                  relayGameObject.FieldConfigurations.Add(new RelayGameObjectFieldConfiguration()
                  {
                    ComponentName = componentKey,
                    FieldName = fields[x].Name
                  });
                  EditorUtility.SetDirty(relayGameObject);
                }
              }
              else
              {
                if (selected)
                {
                  var config = relayGameObject.FieldConfigurations.FirstOrDefault(f => f.ComponentName == componentKey && f.FieldName == fields[x].Name);

                  relayGameObject.FieldConfigurations.Remove(config);
                  EditorUtility.SetDirty(relayGameObject);
                }
              }

              //EditorGUILayout.LabelField(new GUIContent(fields[x].FieldType.Name + " (" + UnsafeUtility.SizeOf(fields[x].FieldType).ToString() + ")"), GUILayout.Width(fieldInfoColumnWidths[2]));
              EditorGUILayout.LabelField(new GUIContent(type.Name), GUILayout.Width(fieldInfoColumnWidths[2]));

              if (!fieldReplicationUpdateMethod.ContainsKey(fieldKey))
              {
                fieldReplicationUpdateMethod.Add(fieldKey, 0);
              }

              if (selected)
              {
                DrawUpdateMethodDropdown(relayGameObject, componentKey, fields[x].Name, fieldType);
              }

              EditorGUILayout.EndHorizontal();
            }
          }
        }
      }

      RelayEditorUtils.BottonPad(8);

      GUILayout.Label($"On Initialization Events", RelayEditorUtils.HeaderFont());
      EditorGUILayout.Space();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PropertyField(onInitializeLocalTrue);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PropertyField(onInitializeLocalFalse);
      EditorGUILayout.EndHorizontal();

      RelayEditorUtils.BottonPad(4);

      RelayEditorUtils.BottonPad(8);

      //GUILayout.Label($"Information", RelayEditorUtils.HeaderFont());
      //RelayEditorUtils.GuiLine();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Byte count:", "The byte count this object will add to the overall network state."), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //EditorGUILayout.LabelField(stateSize.ToString() + " bytes", RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      //EditorGUILayout.EndHorizontal();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Registered:", "The byte count this object will add to the overall network state."), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //EditorGUILayout.LabelField(relayGameObject.AddedToRepository.ToString(), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      //EditorGUILayout.EndHorizontal();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Local:", "The byte count this object will add to the overall network state."), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //EditorGUILayout.LabelField(relayGameObject.IsLocal.ToString(), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      //EditorGUILayout.EndHorizontal();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Network instance id:", "The ID associated to this object for network communication."), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //EditorGUILayout.LabelField(relayGameObject.NetworkInstanceId.ToString(), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[1]));
      //EditorGUILayout.EndHorizontal();

      //EditorGUILayout.BeginHorizontal();
      //EditorGUILayout.LabelField(new GUIContent("Relay instance id:", "The ID associated to this object for network communication."), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //EditorGUILayout.LabelField("(int)" + relayGameObject.RelayInstanceId.ToString(), RelayEditorUtils.LabelFont(), GUILayout.Width(80));
      //EditorGUILayout.LabelField("(uint)" + ((uint)relayGameObject.RelayInstanceId).ToString(), RelayEditorUtils.LabelFont(), GUILayout.Width(160));
      //EditorGUILayout.EndHorizontal();

      //RelayEditorUtils.GuiLine();

      //var serializableFields = relayGameObject.GetAllSerializableFields().Select(x => x.Value).ToArray();

      //for (int i = 0; i < serializableFields.Length; i++)
      //{
      //  var componentFields = serializableFields[i].Select(x => x.Value).ToArray();

      //  for (int x = 0; x < componentFields.Length; x++)
      //  {
      //    EditorGUILayout.BeginHorizontal();
      //    EditorGUILayout.LabelField(new GUIContent(componentFields[x].Name), RelayEditorUtils.LabelFont(), GUILayout.Width(fieldInfoColumnWidths[0]));
      //    EditorGUILayout.EndHorizontal();
      //  }

      //}

      serializedObject.ApplyModifiedProperties();
      serializedObject.Update();
    }

    private static Type[] drawableUpdateMethodDropdownForTypes = new Type[] {
    typeof(float),
    typeof(int),
    typeof(byte),
    typeof(short),
    typeof(long),
    typeof(Vector2),
    typeof(Vector4),
    typeof(Vector3),
    typeof(Quaternion)
  };

    public void DrawUpdateMethodDropdown(RelayGameObject relayGameObject, string componentKey, string fieldKey, Type fieldType)
    {
      if (drawableUpdateMethodDropdownForTypes.Contains(fieldType))
      {
        var config = relayGameObject.FieldConfigurations.FirstOrDefault(x => x.ComponentName == componentKey && x.FieldName == fieldKey);
        if (config == null)
        {
          return;
        }

        var previous = config.PropertyUpdateType;

        var selectedUpdateMode = EditorGUILayout.Popup((int)config.PropertyUpdateType, Enum.GetNames(typeof(PropertyUpdateType)));
        if (selectedUpdateMode != (int)previous)
        {
          EditorUtility.SetDirty(relayGameObject);
        }

        config.PropertyUpdateType = (PropertyUpdateType)selectedUpdateMode;

        if (config.PropertyUpdateType != PropertyUpdateType.Immediate)
        {
          GUILayout.Label("Speed:");
          var timeMultiplier = EditorGUILayout.TextField(config.LerpDeltaTimeMultiplier.ToString());

          float parsedTimeMultiplier = 0.0f;
          if (float.TryParse(timeMultiplier, out parsedTimeMultiplier))
          {
            if (timeMultiplier != config.LerpDeltaTimeMultiplier.ToString())
            {
              EditorUtility.SetDirty(relayGameObject);
            }
            config.LerpDeltaTimeMultiplier = parsedTimeMultiplier;
          }
        }
      }
      else
      {
      }
    }

    private void DrawBuiltInStateReplicationOptions(RelayGameObject relayGameObject)
    {
      GUILayout.Label($"    Lifecycle", RelayEditorUtils.HeaderFont());

      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        Instantiate per Client", "Determines whether Relay should automatically broadcast the instantiation of this gameobject across all clients."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      var newManagedActivityValue = EditorGUILayout.Toggle(relayGameObject.InstantiatePerClient);
      if (newManagedActivityValue && !relayGameObject.InstantiatePerClient)
      {
        EditorUtility.SetDirty(relayGameObject);
        relayGameObject.InstantiatePerClient = true;
      }
      else if (!newManagedActivityValue && relayGameObject.InstantiatePerClient)
      {
        EditorUtility.SetDirty(relayGameObject);
        relayGameObject.InstantiatePerClient = false;
      }
      EditorGUILayout.EndHorizontal();

      //if ((relayGameObject.GetComponent<Rigidbody>() is var rb && rb != null) || (relayGameObject.GetComponent<Rigidbody2D>() is var rb2D && rb2D != null))
      //{
      //  EditorGUILayout.BeginHorizontal();
      //  EditorGUILayout.LabelField(new GUIContent("        Dynamic Owner", "When a client's RelayGameObject collides with this object, it will take ownership of this object, meaning that it can update its state. This allows for smoother interactions between players and in-game world objects that have physics enabled. Don't enable for player characters, otherwise your client loses ownership of it when touching another player's object."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      //  var newOptimizeNetworkPhysicsValue = EditorGUILayout.Toggle(relayGameObject.OptimizeNetworkPhysics);
      //  if (newOptimizeNetworkPhysicsValue && !relayGameObject.OptimizeNetworkPhysics)
      //  {
      //    EditorUtility.SetDirty(relayGameObject);
      //    relayGameObject.OptimizeNetworkPhysics = true;
      //  }
      //  else if (!newOptimizeNetworkPhysicsValue && relayGameObject.OptimizeNetworkPhysics)
      //  {
      //    EditorUtility.SetDirty(relayGameObject);
      //    relayGameObject.OptimizeNetworkPhysics = false;
      //  }
      //  EditorGUILayout.EndHorizontal();
      //}

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(new GUIContent("        Priority", "Determines how often Relay broadcasts the state of this object. Player characters should be high priority, lower priorities can be assigned to world & miscellaneous objects."), RelayEditorUtils.FieldNameFont(true), GUILayout.Width(fieldInfoColumnWidths[0] + 50));
      var previous = relayGameObject.BroadcastPriority;
      var selectedBroadcastPriority = EditorGUILayout.Popup((int)relayGameObject.BroadcastPriority, Enum.GetNames(typeof(RelayBroadcastPriority)));
      if (selectedBroadcastPriority != (int)previous)
      {
        EditorUtility.SetDirty(relayGameObject);
      }

      relayGameObject.BroadcastPriority = (RelayBroadcastPriority)selectedBroadcastPriority;
      EditorGUILayout.EndHorizontal();

      //  RelayEditorUtils.BottonPad(4);
      //  EditorGUILayout.BeginHorizontal();
      //  GUILayout.Label($"    Info", RelayEditorUtils.HeaderFont());
      //  EditorGUILayout.EndHorizontal();
      //  EditorGUILayout.BeginHorizontal();
      //  GUILayout.Label($"      RelayInstanceId: {relayGameObject.RelayInstanceId}", RelayEditorUtils.HeaderFont());
      //  EditorGUILayout.EndHorizontal();
      //  if (GameServerClient.IsConnected)
      //  {
      //    EditorGUILayout.BeginHorizontal();
      //    GUILayout.Label($"      Owner Client Id: {relayGameObject.OwnerClientId}", RelayEditorUtils.HeaderFont());
      //    EditorGUILayout.EndHorizontal();
      //    EditorGUILayout.BeginHorizontal();
      //    GUILayout.Label($"      Initialized: {relayGameObject.Initialized}", RelayEditorUtils.HeaderFont());
      //    EditorGUILayout.EndHorizontal();
      //    EditorGUILayout.BeginHorizontal();
      //    GUILayout.Label($"      IsLocal: {relayGameObject.IsLocal}", RelayEditorUtils.HeaderFont());
      //    EditorGUILayout.EndHorizontal();
      //    EditorGUILayout.BeginHorizontal();
      //    GUILayout.Label($"      NetworkInstanceId: {relayGameObject.NetworkInstanceId}", RelayEditorUtils.HeaderFont());
      //    EditorGUILayout.EndHorizontal();
      //    EditorGUILayout.BeginHorizontal();
      //    GUILayout.Label($"      IsDirty: {relayGameObject.IsDirty}", RelayEditorUtils.HeaderFont());
      //    EditorGUILayout.EndHorizontal();
      //    RelayEditorUtils.BottonPad(4);
      //  }
    }
  }
}