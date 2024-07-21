using Relay.Toolkit.Networking;
using System;
using System.Reflection;
using UnityEngine;

namespace BitterShark.Relay
{
  public static class RelayRpc
  {
    public static void RPC(Action method, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, null);
    }

    public static void RPC<T>(Action<T> method, T param1, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1 });
    }

    public static void RPC<T1, T2>(Action<T1, T2> method, T1 param1, T2 param2, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2 });
    }

    public static void RPC<T1, T2, T3>(Action<T1, T2, T3> method, T1 param1, T2 param2, T3 param3, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2, param3 });
    }

    public static void RPC<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 param1, T2 param2, T3 param3, T4 param4, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4 });
    }

    public static void RPC<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5 });
    }

    public static void RPC<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5, param6 });
    }

    public static void RPC<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, bool broadcast = true)
    {
      Execute(method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5, param6, param7 });
    }

    public static void RPC(ushort clientIdOverride, Action method, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, null);
    }

    public static void RPC<T>(ushort clientIdOverride, Action<T> method, T param1, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1 });
    }

    public static void RPC<T1, T2>(ushort clientIdOverride, Action<T1, T2> method, T1 param1, T2 param2, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2 });
    }

    public static void RPC<T1, T2, T3>(ushort clientIdOverride, Action<T1, T2, T3> method, T1 param1, T2 param2, T3 param3, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2, param3 });
    }

    public static void RPC<T1, T2, T3, T4>(ushort clientIdOverride, Action<T1, T2, T3, T4> method, T1 param1, T2 param2, T3 param3, T4 param4, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4 });
    }

    public static void RPC<T1, T2, T3, T4, T5>(ushort clientIdOverride, Action<T1, T2, T3, T4, T5> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5 });
    }

    public static void RPC<T1, T2, T3, T4, T5, T6>(ushort clientIdOverride, Action<T1, T2, T3, T4, T5, T6> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5, param6 });
    }

    public static void RPC<T1, T2, T3, T4, T5, T6, T7>(ushort clientIdOverride, Action<T1, T2, T3, T4, T5, T6, T7> method, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, bool broadcast = true)
    {
      Execute(clientIdOverride, method.Method, method.Target, broadcast, new object[] { param1, param2, param3, param4, param5, param6, param7 });
    }

    private static void Execute(MethodInfo methodInfo, object target, bool broadcast, object[] parameters)
    {
      var relayGameObject = ((Component)target).GetComponent<RelayGameObject>();

      if (relayGameObject == null)
      {
        Debug.LogError($"Trying to execute an RPC method on a GameObject that doesn't have a RelayGameObject component associated to it is not possible");
      }

      if (!relayGameObject.InstantiatePerClient)
      {
        broadcast = false;
      }

      MessageQueues.PendingClientRpcRequestQueue.Push(new ClientRpcRequest()
      {
        Broadcast = broadcast,
        ReceiverClientId = relayGameObject.OwnerClientId,
        ComponentId = target.GetType().Name.GetHashCode(),
        MethodId = methodInfo.Name.GetHashCode(),
        Parameters = parameters,
        RelayGameObjectId = relayGameObject.NetworkInstanceId,
        SenderClientId = GameServerClient.ClientId,
      });
    }

    private static void Execute(ushort clientIdOverride, MethodInfo methodInfo, object target, bool broadcast, object[] parameters)
    {
      var relayGameObject = ((Component)target).GetComponent<RelayGameObject>();
      if (relayGameObject == null)
      {
        Debug.LogError($"Trying to execute an RPC method on a GameObject that doesn't have a RelayGameObject component associated to it is not possible");
      }

      if (!relayGameObject.InstantiatePerClient)
      {
        broadcast = false;
      }

      MessageQueues.PendingClientRpcRequestQueue.Push(new ClientRpcRequest()
      {
        Broadcast = broadcast,
        ReceiverClientId = clientIdOverride,
        ComponentId = target.GetType().Name.GetHashCode(),
        MethodId = methodInfo.Name.GetHashCode(),
        Parameters = parameters,
        RelayGameObjectId = relayGameObject.NetworkInstanceId,
        SenderClientId = GameServerClient.ClientId,
      });
    }
  }
}
