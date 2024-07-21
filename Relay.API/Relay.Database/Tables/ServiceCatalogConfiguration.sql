create table [Relay].[ServiceCatalogConfiguration]
(
  [ServiceCatalogId] int identity(1, 1) not null,
  [MaxConcurrentPlayers] int not null,
  [MaxPlayersPerGameServer] int not null,
  [GameStateUpdateChunkCount] int not null,
  [ClientRpcUpdateChunkCount] int not null,
  [BroadcastTickRate] int not null,
  [ReceiveTickRate] int not null,

  constraint PK_Relay_ServiceCatalogConfiguration primary key ([ServiceCatalogId])
)
