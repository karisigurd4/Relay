using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BitterShark.Relay
{
  public static class GameStateRepository
  {
    public static int NetworkInstanceIdIndex = 0;

    private static Dictionary<int, RelayGameObject> relayGameObjects = new Dictionary<int, RelayGameObject>();

    public static void Clear()
    {
      lock (relayGameObjects)
      {
        relayGameObjects.Clear();
      }
    }

    public static int GetNextNetworkInstanceId()
    {
      return NetworkInstanceIdIndex++;
    }

    public static void RegisterRelayGameObject(RelayGameObject relayGameObject, bool localRegistration = false)
    {
      lock (relayGameObjects)
      {
        if (!relayGameObjects.ContainsKey(relayGameObject.NetworkInstanceId))
        {
          if (localRegistration && relayGameObject.InstantiatePerClient)
          {
            relayGameObject.NetworkInstanceId = GetNextNetworkInstanceId();
          }
          else if (!relayGameObject.InstantiatePerClient)
          {
            relayGameObject.NetworkInstanceId = relayGameObject.RelayInstanceId;

            if (!GameServerClient.IsLowestIdClient())
            {
              relayGameObject.IsLocal = false;
              relayGameObject.OwnerClientId = GameServerClient.GetLowestIdClient();
            }
            else
            {
              relayGameObject.OwnerClientId = GameServerClient.ClientId;
            }
          }

          if ((localRegistration && relayGameObject.InstantiatePerClient) || (!relayGameObject.InstantiatePerClient && localRegistration))
          {
            relayGameObject.OnInitializeLocalTrue.Invoke();
          }
          else
          {
            relayGameObject.OnInitializeLocalFalse.Invoke();
          }

          var rpcMethods = new Dictionary<int, MethodInfo>();
          var components = relayGameObject.GetComponents(typeof(Component));

          for (int i = 0; i < components.Length; i++)
          {
            var componentRpcMethods = components[i]
              .GetType()
              .GetMethods()
              .ToArray();

            for (int x = 0; x < componentRpcMethods.Length; x++)
            {
              RpcMethodCache.AddRpcMethodDefinition(relayGameObject.NetworkInstanceId, components[i], componentRpcMethods[x]);
            }
          }

          if (!relayGameObjects.ContainsKey(relayGameObject.NetworkInstanceId))
          {
            relayGameObjects.Add(relayGameObject.NetworkInstanceId, relayGameObject);
          }
        }
      }
    }

    public static void HandleClientDisconnected(ushort clientId)
    {
      lock (relayGameObjects)
      {
        var clientRelayGameObjects = relayGameObjects
          .Where(x => x.Value.OwnerClientId == clientId && x.Value.DestroyOnClientDisconnect)
          .Select(x => x.Value)
          .ToArray();

        for (int i = 0; i < clientRelayGameObjects.Length; i++)
        {
          if (clientRelayGameObjects[i].InstantiatePerClient)
          {
            UnregisterRelayGameObject(clientRelayGameObjects[i]);
            clientRelayGameObjects[i].MarkDestroy = true;
          }
        }
      }
    }

    public static void UnregisterRelayGameObject(RelayGameObject relayGameObject)
    {
      lock (relayGameObjects)
      {
        if (relayGameObjects.ContainsKey(relayGameObject.NetworkInstanceId))
        {
          relayGameObjects.Remove(relayGameObject.NetworkInstanceId);
        }
      }
    }

    public static RelayGameObject GetRelayGameObjectByNetworkInstanceId(int networkInstanceId)
    {
      lock (relayGameObjects)
      {
        if (!relayGameObjects.ContainsKey(networkInstanceId))
        {
          return null;
        }

        return relayGameObjects[networkInstanceId];
      }
    }

    public static RelayGameObject[] GetState(bool localState = false)
    {
      lock (relayGameObjects)
      {
        if (localState)
        {
          var dirtyGameObjects = relayGameObjects.Values
            .Where(x =>
              x.IsLocal &&
              x.IsDirty &&
              x.NetworkInstanceId != 0 &&
              x.RelayInstanceId != 0
            ).ToArray();

          for (int i = 0; i < dirtyGameObjects.Length; i++)
          {
            dirtyGameObjects[i].IsDirty = false;
          }

          return dirtyGameObjects;
        }
        else
        {
          return relayGameObjects.Values.ToArray();
        }
      }
    }

    public static Dictionary<int, RelayGameObject> GetAll()
    {
      return relayGameObjects;
    }
  }
}
