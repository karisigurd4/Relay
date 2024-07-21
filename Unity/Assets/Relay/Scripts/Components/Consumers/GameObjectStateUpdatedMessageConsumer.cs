using UnityEngine;

namespace BitterShark.Relay
{
  public class GameObjectStateUpdatedMessageConsumer : MessageBusConsumer<GameObjectStateUpdatedMessage>
  {
    // Arbitrary constant to stop updaing ownership if it changed recently on a local instance
    private const float OwnerTimeChangeDelay = 6.0f;

    public override void OnConsumeMessage(GameObjectStateUpdatedMessage message)
    {
      var relayGameObject = GameStateRepository.GetRelayGameObjectByNetworkInstanceId(message.GameObjectState.NetworkInstanceId);
      if (relayGameObject == null)
      {
        //Debug.LogError($"Received message to update a RelayGameObject with network instance id: {message.GameObjectState.NetworkInstanceId} but none such is registered");
        return;
      }

      if (relayGameObject.IsLocal && Time.time - relayGameObject.OwnerChangedAt > OwnerTimeChangeDelay && relayGameObject.OwnerClientId != message.GameObjectState.ClientId)
      {
        Debug.Log($"Ownership changed new owner: {message.GameObjectState.ClientId}");
        relayGameObject.OwnerChangedAt = Time.time;
        relayGameObject.OwnerClientId = message.GameObjectState.ClientId;
        relayGameObject.IsLocal = message.GameObjectState.ClientId == GameServerClient.ClientId;
        relayGameObject.Initialized = true;
      }

      relayGameObject.NetworkInstanceId = message.GameObjectState.NetworkInstanceId;

      if (relayGameObject.IsLocal)
      {
        return;
      }

      relayGameObject.ApplyFieldStates(message.GameObjectState.State);
    }
  }
}
