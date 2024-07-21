namespace Relay.GameServer.DataModel
{
  public class ServiceCatalogConfiguration
  {
    public int ServiceCatalogId { get; set; }
    public int MaxConcurrentPlayers { get; set; }
    public int MaxPlayersPerGameServer { get; set; }
    public int GameStateUpdateChunkCount { get; set; }
    public int ClientRpcUpdateChunkCount { get; set; }
    public int BroadcastTickRate { get; set; }
    public int ReceiveTickRate { get; set; }
  }
}
