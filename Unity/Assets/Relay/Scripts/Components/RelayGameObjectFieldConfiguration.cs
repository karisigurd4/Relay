using System;
using UnityEngine;

namespace BitterShark.Relay
{
  [Serializable]
  public class RelayGameObjectFieldConfiguration
  {
    [SerializeField] public string ComponentName;
    [SerializeField] public string FieldName;
    [SerializeField] public PropertyUpdateType PropertyUpdateType;
    [SerializeField] public float LerpDeltaTimeMultiplier = 30.0f;
    public int ComponentHashCode => ComponentName.GetHashCode();
    public int FieldHashCode => FieldName.GetHashCode();
  }
}
