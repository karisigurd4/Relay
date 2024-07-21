using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitterShark.Relay
{
  public class RelayGameObjectManager : MonoBehaviour
  {
    private static RelayGameObjectManager _instance;
    public static RelayGameObjectManager Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;

      relayGameObjectsCache = new Dictionary<int, RelayGameObject>();
      var relayGameObjects = FindObjectsOfType<RelayGameObject>();

      for (int i = 0; i < relayGameObjects.Length; i++)
      {
        if (!relayGameObjectsCache.ContainsKey(relayGameObjects[i].RelayInstanceId))
        {
          relayGameObjectsCache.Add(relayGameObjects[i].GenerateRelayInstanceId(), relayGameObjects[i]);
        }
      }

      var objs = Resources.LoadAll<RelayGameObject>("RelayPrefabs");

      for (int i = 0; i < objs.Length; i++)
      {
        var key = objs[i].GenerateRelayInstanceId();

        if (!relayGameObjectsCache.ContainsKey(key))
        {
          relayGameObjectsCache.Add(key, objs[i]);
        }
      }
    }

    [HideInInspector]
    public Dictionary<int, RelayGameObject> relayGameObjectsCache;

    public void Register(RelayGameObject relayGameObject)
    {
      if (!relayGameObjectsCache.ContainsKey(relayGameObject.RelayInstanceId))
      {
        relayGameObjectsCache.Add(relayGameObject.RelayInstanceId, relayGameObject);
      }
    }

    public RelayGameObject GetRelayGameObjectByInstanceId(int id)
    {
      if (!relayGameObjectsCache.ContainsKey(id))
      {
        return null;
      }

      return relayGameObjectsCache[id];
    }

    public RelayGameObject[] GetAllInstantiatedRelayGameObjects()
    {
      return relayGameObjectsCache.Select(x => x.Value).Where(x => x.NetworkInstanceId != 0).ToArray();
    }
  }
}
