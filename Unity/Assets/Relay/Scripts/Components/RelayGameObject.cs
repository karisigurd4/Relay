using Relay.Toolkit.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace BitterShark.Relay
{
  [ExecuteAlways]
  [AddComponentMenu("Relay/Relay Game Object")]
  public class RelayGameObject : MonoBehaviour
  {
    const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

    private int PositionKeyHashCode, RotationKeyHashCode, VelocityKeyHashCode, IsKinematicHashCode;

    private int RelayGameObjectNameHashcode = 0;

    public bool IsDirty = true;
    [HideInInspector]
    public int NetworkInstanceId;
    [HideInInspector]
    public bool IsLocal = true;

    public ushort OwnerClientId;
    public int OwnerPlayerId;

    [SerializeField]
    public int RelayInstanceId;

    public bool InstantiatePerClient = true;
    public bool OptimizeNetworkPhysics = false;
    public bool DestroyOnClientDisconnect = true;

    private Vector3 optimizedPhysicsLastUpdatedPosition = Vector3.zero;

    public bool MarkDestroy = false;
    public bool Initialized = false;

    public float OwnerChangedAt = 0;

    public RelayBroadcastPriority BroadcastPriority = RelayBroadcastPriority.High;

    [SerializeField]
    public List<RelayGameObjectFieldConfiguration> FieldConfigurations = new List<RelayGameObjectFieldConfiguration>();

    private Dictionary<int, Dictionary<int, object>> latestState = new Dictionary<int, Dictionary<int, object>>();

    private Rigidbody rigidbodyReference;
    private Rigidbody2D rigidbody2DReference;

    private Dictionary<string, RelayGameObjectFieldState> relayGameObjectState = new Dictionary<string, RelayGameObjectFieldState>();
    private Dictionary<int, Dictionary<int, MemberInfo>> hashcodesToComponentHashFields = new Dictionary<int, Dictionary<int, MemberInfo>>();
    private Dictionary<int, Component> hashcodesToComponents = new Dictionary<int, Component>();

    private Vector3 positionTarget;
    private Quaternion rotationTarget;
    private Vector3 velocityTarget;

    private float lastOptimizedNetworkPhysicsUpdate = 0;
    private int markDirty = 0;

    public UnityEvent OnInitializeLocalTrue;
    public UnityEvent OnInitializeLocalFalse;

    private static Type[] serializableVectorFields = new Type[]
    {
    typeof(Vector2),
    typeof(Vector3),
    typeof(Vector4)
    };

    private void Awake()
    {
    }

    public int GenerateRelayInstanceId()
    {
      var name = gameObject.name;
      if (gameObject.name.Contains(' '))
      {
        name = name.Split(' ')[0];
      }

      if (gameObject.name.Contains('('))
      {
        name = name.Split('(')[0];
      }

      var nameHashCode = name.GetHashCode();

      return nameHashCode;
    }

    private void Start()
    {
      OwnerChangedAt = Time.time;

      StartInitialize();

      if (RelayGameObjectManager.Instance != null)
      {
        RelayGameObjectManager.Instance.Register(this);
      }
    }

    public void StartInitialize()
    {
      RelayInstanceId = GenerateRelayInstanceId();

      NetworkInstanceId = 0;

      Initialized = false;

      RelayGameObjectNameHashcode = nameof(RelayGameObject).GetHashCode();

      hashcodesToComponents = GetComponents(typeof(Component))
        .ToDictionary(x => x.GetType().Name.GetHashCode(), x => x);

      hashcodesToComponentHashFields = GetComponents(typeof(Component))
        .ToDictionary(x => x.GetType().Name.GetHashCode(), x => GetComponentHashedFieldReferences(x));
    }

    private Dictionary<int, MemberInfo> GetComponentHashedFieldReferences(Component component)
    {
      return component
        .GetType()
        .GetFields(bindingFlags)
        .Cast<MemberInfo>()
        .Concat(component.GetType().GetProperties(bindingFlags))
        .ToDictionary(x => x.Name.GetHashCode(), x => x);
    }

    public Dictionary<int, Dictionary<int, MemberInfo>> GetAllSerializableFields()
    {
      return hashcodesToComponentHashFields;
    }

    private Component GetComponentByNameHashcode(int hashcode)
    {
      if (!hashcodesToComponents.ContainsKey(hashcode))
      {
        return null;
      }

      return hashcodesToComponents[hashcode];
    }

    private Dictionary<int, MemberInfo> GetComponentFieldInfoByComponentNameHashId(int hashcode)
    {
      if (!hashcodesToComponentHashFields.ContainsKey(hashcode))
      {
        return null;
      }

      return hashcodesToComponentHashFields[hashcode];
    }

    private void OnEnable()
    {
      rigidbodyReference = GetComponent<Rigidbody>();
      rigidbody2DReference = GetComponent<Rigidbody2D>();
    }

    public bool IsOwner()
    {
      return IsLocal;
    }

    public void ApplyFieldStates(RelayGameObjectFieldState[] states)
    {
      if (states == null)
      {
        return;
      }

      for (int i = 0; i < states.Length; i++)
      {
        if (!latestState.ContainsKey(states[i].ComponentHashCode))
        {
          latestState.Add(states[i].ComponentHashCode, new Dictionary<int, object>());
        }

        if (!latestState[states[i].ComponentHashCode].ContainsKey(states[i].FieldHashCode))
        {
          latestState[states[i].ComponentHashCode].Add(states[i].FieldHashCode, states[i].Value);
        }
        else
        {
          latestState[states[i].ComponentHashCode][states[i].FieldHashCode] = states[i].Value;
        }
      }
    }

    public void Update()
    {
      if (Application.isPlaying && GameServerClient.IsConnected)
      {
        if (!InstantiatePerClient && !GameServerClient.IsLowestIdClient())
        {
          IsLocal = false;
        }

        if (OptimizeNetworkPhysics && Time.time - OwnerChangedAt > 6.0f)
        {
          IsLocal = (GameServerClient.IsConnected && OwnerClientId == GameServerClient.ClientId);
        }

        if (IsLocal)
        {
          OwnerPlayerId = RelayPlayerManager.PlayerId;

          if (markDirty > 0)
          {
            markDirty -= 1;
          }

          if (!OptimizeNetworkPhysics && markDirty <= 0)
          {
            IsDirty = true;

            switch (BroadcastPriority)
            {
              case RelayBroadcastPriority.High: break;
              case RelayBroadcastPriority.Medium: markDirty = 3; break;
              case RelayBroadcastPriority.Low: markDirty = 6; break;
            }

          }
          else
          {
            SetOptimizedNetworkPhysicsDirtyFlag();
          }

          OwnerClientId = GameServerClient.ClientId;

          for (int i = 0; i < FieldConfigurations.Count; i++)
          {
            var affectedComponent = GetComponentByNameHashcode(FieldConfigurations[i].ComponentHashCode);

            var affectedComponentFieldInfo = GetComponentFieldInfoByComponentNameHashId(FieldConfigurations[i].ComponentHashCode);
            if (affectedComponentFieldInfo == null)
            {
              // Component probably no longer attached
              continue;
            }

            AddOrUpdateStateField
            (
              FieldConfigurations[i].ComponentName + "." + FieldConfigurations[i].FieldName,
              FieldConfigurations[i].ComponentName,
              FieldConfigurations[i].FieldName,
              GetMemberInfoValue(affectedComponentFieldInfo[FieldConfigurations[i].FieldHashCode], affectedComponent)
            );
          }
        }
        else
        {
          if (!OptimizeNetworkPhysics || Time.time - OwnerChangedAt > 6.0f)
          {
            IsLocal = OwnerClientId == GameServerClient.ClientId;
          }

          if (InstantiatePerClient && GameServerClient.IsDisconnectedClient(OwnerClientId))
          {
            Destroy(gameObject);
          }
          else if (!InstantiatePerClient && !OptimizeNetworkPhysics)
          {
            OwnerClientId = GameServerClient.GetLowestIdClient();
          }

          for (int i = 0; i < latestState.Keys.Count; i++)
          {
            var componentHashCode = latestState.Keys.ToArray()[i];

            var affectedComponent = GetComponentByNameHashcode(componentHashCode);
            if (affectedComponent == null)
            {
              //Debug.LogWarning($"Warning: Received state with component id {componentHashCode} but none such is attached to the GameObject.");
              continue;
            }

            for (int x = 0; x < latestState[componentHashCode].Keys.Count; x++)
            {
              var fieldInfoHashCode = latestState[componentHashCode].Keys.ToArray()[x];

              var affectedComponentFieldInfo = GetComponentFieldInfoByComponentNameHashId(componentHashCode);
              if (affectedComponentFieldInfo == null)
              {
                Debug.LogWarning($"Warning: Received state with component id {componentHashCode} and a field hash code {fieldInfoHashCode} but no field with that hashcode is associated to the gameobject.");
                continue;
              }

              var fieldConfiguration = FieldConfigurations.FirstOrDefault(x => x.FieldHashCode == fieldInfoHashCode && x.ComponentHashCode == componentHashCode);
              if (fieldConfiguration == null)
              {
                Debug.LogError($"If using ParallelSync, make sure it has reloaded.");
                continue;
              }

              var targetValue = latestState[componentHashCode][fieldInfoHashCode];

              if (fieldConfiguration.PropertyUpdateType == PropertyUpdateType.Lerp)
              {
                var currentValue = GetMemberInfoValue(affectedComponentFieldInfo[fieldInfoHashCode], affectedComponent);

                if (serializableVectorFields.Contains(currentValue.GetType()))
                {
                  if (currentValue.GetType() == typeof(Vector2))
                  {
                    targetValue = Vector2.Lerp((Vector2)currentValue, (Vector2)targetValue, Time.deltaTime * fieldConfiguration.LerpDeltaTimeMultiplier);
                  }
                  else if (currentValue.GetType() == typeof(Vector3))
                  {
                    targetValue = Vector3.Lerp((Vector3)currentValue, (Vector3)targetValue, Time.deltaTime * fieldConfiguration.LerpDeltaTimeMultiplier);
                  }
                  else if (currentValue.GetType() == typeof(Vector4))
                  {
                    targetValue = Vector4.Lerp((Vector4)currentValue, (Vector4)targetValue, Time.deltaTime * fieldConfiguration.LerpDeltaTimeMultiplier);
                  }
                }
                else if (currentValue.GetType() == typeof(Quaternion))
                {
                  targetValue = Quaternion.Lerp((Quaternion)currentValue, (Quaternion)targetValue, Time.deltaTime * fieldConfiguration.LerpDeltaTimeMultiplier);
                }
                else
                {
                  try
                  {
                    targetValue = Mathf.Lerp((float)currentValue, (float)targetValue, Time.deltaTime * fieldConfiguration.LerpDeltaTimeMultiplier);
                  }
                  catch
                  {
                    // I don't know, it sometimes bugs out when you use for instance the System.Single instead of float but only just sometimes, and sometimes not. 
                    // TODO: What
                  }
                }
              }

              SetMemberInfoValue(affectedComponentFieldInfo[fieldInfoHashCode], affectedComponent, targetValue);
            }
          }
        }
      }

      if (!OptimizeNetworkPhysics || Time.time - OwnerChangedAt > 6.0f)
      {
        if ((!InstantiatePerClient || IsLocal) && !Initialized && GameServerClient.IsConnected)
        {
          GameStateRepository.RegisterRelayGameObject(this, true);
          Initialized = true;
        }
      }

      if (MarkDestroy)
      {
        Destroy(gameObject);
      }
    }

    private void SetOptimizedNetworkPhysicsDirtyFlag()
    {
      if (rigidbodyReference != null)
      {
        if (markDirty <= 0)
        {
          optimizedPhysicsLastUpdatedPosition = transform.position;
          IsDirty = true;
          lastOptimizedNetworkPhysicsUpdate = Time.time;
        }
      }
      else if (rigidbody2DReference != null)
      {
        if (markDirty <= 0)
        {
          IsDirty = true;
          markDirty = 3;
          lastOptimizedNetworkPhysicsUpdate = Time.time;
        }
      }
    }

    private static object GetMemberInfoValue(MemberInfo memberInfo, Component component)
    {
      if (memberInfo.MemberType == MemberTypes.Property)
      {
        return ((PropertyInfo)memberInfo).GetValue(component);
      }
      else
      {
        return ((FieldInfo)memberInfo).GetValue(component);
      }
    }

    private static void SetMemberInfoValue(MemberInfo memberInfo, Component component, object value)
    {
      if (memberInfo.MemberType == MemberTypes.Property)
      {
        try
        {
          ((PropertyInfo)memberInfo).SetValue(component, value);
        }
        catch (Exception e)
        {
          Debug.LogError($"Error {e} when trying to set property {memberInfo.Name}. Running ParallelSync and forgot to save?");
        }
      }
      else
      {
        try
        {
          ((FieldInfo)memberInfo).SetValue(component, value);
        }
        catch (Exception e)
        {
          Debug.LogError($"Error {e} when trying to set field {memberInfo.Name}. Running ParallelSync and forgot to save?");
        }
      }
    }

    private void AddOrUpdateStateField(string key, string componentName, string fieldName, object value)
    {
      if (!relayGameObjectState.ContainsKey(key))
      {
        relayGameObjectState.Add(key, new RelayGameObjectFieldState()
        {
          Value = (object)value,
          FieldHashCode = fieldName.GetHashCode(),
          ComponentHashCode = componentName.GetHashCode()
        });
      }

      relayGameObjectState[key].Value = (object)value;
    }

    public void ChangeOwnerClient(ushort clientId)
    {
      OwnerClientId = clientId;
      IsLocal = (clientId == GameServerClient.ClientId);
    }

    private void OnCollisionStay(Collision collision)
    {
      if (!OptimizeNetworkPhysics || !Initialized || !IsLocal)
      {
        return;
      }

      RelayGameObject touchingObject = null;

      if (collision.collider.gameObject.GetComponent<RelayGameObject>() is var rgo && rgo != null)
      {
        touchingObject = rgo;
      }
      else if (collision.collider.gameObject.GetComponentInChildren<RelayGameObject>() is var crgo && crgo != null)
      {
        touchingObject = crgo;
      }

      var rb = GetComponent<Rigidbody>();
      var touchingRb = GetComponent<Rigidbody>();

      if (rb.velocity.sqrMagnitude > touchingRb.velocity.sqrMagnitude)
      {
        return;
      }

      if (touchingObject != null)
      {
        if (touchingObject.OwnerClientId == GameServerClient.ClientId && !IsLocal)
        {
          ChangeOwnerClient(touchingObject.OwnerClientId);
        }

        OwnerChangedAt = Time.time;
      }
    }

    public RelayGameObjectFieldState[] GetState() => relayGameObjectState.Select(x => x.Value).ToArray();

    public int GetStateSize() => GetState().Select(x => UnsafeUtility.SizeOf(x.Value.GetType())).Sum();
  }
}
