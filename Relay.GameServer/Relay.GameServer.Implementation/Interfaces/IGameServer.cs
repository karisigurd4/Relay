namespace Relay.GameServer.Implementation.Interfaces
{
  using Relay.GameServer.Core.Types;
  using System;

  public interface IGameServer
  {
    void Start(int gameServerId, ushort port, GameServerState gameServerStartState, Guid projectId);
  }
}
