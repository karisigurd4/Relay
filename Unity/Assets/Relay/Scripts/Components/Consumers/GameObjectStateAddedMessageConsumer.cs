using System.Collections.Generic;
using UnityEngine;

namespace BitterShark.Relay
{
  public class GameObjectStateAddedMessageConsumer : MessageBusConsumer<GameObjectStateAddedMessage>
  {
    public List<int> ignoreRelayInstanceIds = new List<int>();

    public override void OnConsumeMessage(GameObjectStateAddedMessage message)
    {
      if (ignoreRelayInstanceIds.Contains(message.GameObjectState.RelayInstanceId))
      {
        return;
      }

      if (GameServerClient.IsDisconnectedClient(message.GameObjectState.ClientId))
      {
        return;
      }

      //Debug.Log($"Adding RelayGameObject with network instance id: {message.GameObjectState.NetworkInstanceId}");

      var instantiatedReference = GameStateRepository.GetRelayGameObjectByNetworkInstanceId(message.GameObjectState.NetworkInstanceId);
      if (instantiatedReference != null)
      {
        if (instantiatedReference.IsLocal)
        {
          return;
        }

        Debug.Log($"Received added message for a {nameof(RelayGameObject)} but one such with a network instance id {message.GameObjectState.NetworkInstanceId} already is registered");
        return;
      }

      var relayGameObject = RelayGameObjectManager.Instance.GetRelayGameObjectByInstanceId(message.GameObjectState.RelayInstanceId);
      if (relayGameObject == null)
      {
        ignoreRelayInstanceIds.Add(message.GameObjectState.RelayInstanceId);
        Debug.LogError($"Cannot add a {nameof(RelayGameObject)} with instance id {message.GameObjectState.RelayInstanceId}");
        return;
      }

      if (!relayGameObject.InstantiatePerClient)
      {
        ignoreRelayInstanceIds.Add(relayGameObject.RelayInstanceId);
        return;
      }

      var instance = Instantiate(relayGameObject);
      instance.NetworkInstanceId = message.GameObjectState.NetworkInstanceId;
      instance.OwnerClientId = message.GameObjectState.ClientId;
      instance.OwnerPlayerId = message.GameObjectState.ApiPlayerId;
      instance.IsLocal = false;
      instance.Initialized = true;

      GameStateRepository.RegisterRelayGameObject(instance, false);

      instance.ApplyFieldStates(message.GameObjectState.State);

      if (!instance.gameObject.activeInHierarchy)
      {
        instance.gameObject.SetActive(true);
      }
    }
  }
}
