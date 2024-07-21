namespace Relay.GameServer.Repository.Implementation
{
  using Relay.GameServer.DataModel;
  using Relay.GameServer.Repository.Interfaces;
  using System.Collections.Generic;
  using System.Linq;

  public class GameObjectStateRepository : IGameObjectStateRepository
  {
    public Dictionary<int, GameObjectState> gameObjectStates = new Dictionary<int, GameObjectState>();

    public GetGameStateResponse GetGameState(GetGameStateRequest request)
    {
      lock (gameObjectStates)
      {
        return new GetGameStateResponse()
        {
          GameState = gameObjectStates?.Values.ToArray() ?? new GameObjectState[] { },
          Success = true
        };
      }
    }

    public HandleClientDisconnectedResponse HandleClientDisconnected(HandleClientDisconnectedRequest request)
    {
      lock (gameObjectStates)
      {
        var clientGameObjects = gameObjectStates
          .Where(x => x.Value.ClientId == request.ClientId)
          .Select(x => x.Key)
          .ToArray();

        for (int i = 0; i < clientGameObjects.Length; i++)
        {
          gameObjectStates.Remove(clientGameObjects[i]);
        }
      }

      return new HandleClientDisconnectedResponse()
      {
        Success = true
      };
    }

    public RemoveGameObjectStateResponse RemoveGameObjectState(RemoveGameObjectStateRequest request)
    {
      return null;
    }

    public UpdateGameObjectStateResponse UpdateGameObjectState(UpdateGameObjectStateRequest request)
    {
      lock (gameObjectStates)
      {
        if (!gameObjectStates.ContainsKey(request.GameObjectState.NetworkInstanceId))
        {
          gameObjectStates.Add(request.GameObjectState.NetworkInstanceId, request.GameObjectState);
          return UpdateGameObjectStateResponseFactory.Create(UpdateGameObjectStateResult.Added);
        }
        else
        {
          gameObjectStates[request.GameObjectState.NetworkInstanceId] = request.GameObjectState;
          return UpdateGameObjectStateResponseFactory.Create(UpdateGameObjectStateResult.Updated);
        }
      }
    }
  }
}
