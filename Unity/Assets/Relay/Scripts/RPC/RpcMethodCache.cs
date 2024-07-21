using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BitterShark.Relay
{
  public static class RpcMethodCache
  {
    private static Dictionary<int, Dictionary<int, RpcMethodDefinition>> rpcMethodDefinitions = new Dictionary<int, Dictionary<int, RpcMethodDefinition>>();

    public static void Clear()
    {
      rpcMethodDefinitions.Clear();
    }

    public static void AddRpcMethodDefinition(int relayGameObjectNetworkId, object fromObject, MethodInfo forMethod)
    {
      var methodHashCode = forMethod.Name.GetHashCode();
      //Debug.Log($"Adding RPC cache for method with hash code {methodHashCode} on RpcHandler with Id {relayGameObjectNetworkId}");

      if (!rpcMethodDefinitions.ContainsKey(relayGameObjectNetworkId))
      {
        rpcMethodDefinitions.Add(relayGameObjectNetworkId, new Dictionary<int, RpcMethodDefinition>());
      }

      if (!rpcMethodDefinitions[relayGameObjectNetworkId].ContainsKey(methodHashCode))
      {
        rpcMethodDefinitions[relayGameObjectNetworkId].Add(methodHashCode, new RpcMethodDefinition()
        {
          MethodInfo = forMethod,
          ObjectReference = fromObject
        });
      }
    }

    public static void ExecuteMethod(int relayNetworkInstanceId, int methodNameHashCode, params object[] parameters)
    {
      if (!rpcMethodDefinitions.ContainsKey(relayNetworkInstanceId))
      {
        Debug.LogError($"Trying to execute an unknown method {methodNameHashCode} on RelayGameObject {relayNetworkInstanceId}");
        return;
      }

      if (!rpcMethodDefinitions[relayNetworkInstanceId].ContainsKey(methodNameHashCode))
      {
        Debug.LogError($"Trying to execute an unknown method {methodNameHashCode} on RelayGameObject {relayNetworkInstanceId}");
        return;
      }

      rpcMethodDefinitions[relayNetworkInstanceId][methodNameHashCode].MethodInfo.Invoke
      (
        rpcMethodDefinitions[relayNetworkInstanceId][methodNameHashCode].ObjectReference,
        parameters
      );
    }
  }
}
