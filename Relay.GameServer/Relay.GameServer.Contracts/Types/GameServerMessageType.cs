namespace Relay.GameServer.Contracts
{
  public enum GameServerMessageType
  {
    ClientConnectionDetails,
    ClientDisconnected,
    ClientRpcRequest,
    ClientGameState,
    ServerGameState,
    GameServerStateUpdated,
    Ping
  }
}
