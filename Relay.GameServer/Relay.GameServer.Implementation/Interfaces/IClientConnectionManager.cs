using Relay.GameServer.Core.Types;
using Riptide;

namespace Relay.GameServer.Implementation.Interfaces
{
  public interface IClientConnectionManager
  {
    ushort[] GetConnectedClientIds();
    void OnClientConnected(ServerConnectedEventArgs eventArgs, GameServerState gameServerState, int gameServerId, Server server);
    void OnClientDisconnected(ushort clientId, int gameId, Server server);
  }
}
