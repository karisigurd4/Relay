-- Free
insert into [Relay].[ServiceCatalogConfiguration]
(
  [ServiceCatalogId],
  [MaxConcurrentPlayers],
  [MaxPlayersPerGameServer],
  [GameStateUpdateChunkCount],
  [ClientRpcUpdateChunkCount],
  [BroadcastTickRate],
  [ReceiveTickRate]

)
values 
(
  1,
  20,
  4,
  32,
  16,
  40,
  24
)

-- Standard
insert into [Relay].[ServiceCatalogConfiguration]
(
  [ServiceCatalogId],
  [MaxConcurrentPlayers],
  [MaxPlayersPerGameServer],
  [GameStateUpdateChunkCount],
  [ClientRpcUpdateChunkCount],
  [BroadcastTickRate],
  [ReceiveTickRate]
)
values 
(
  2,
  100,
  8,
  64,
  32,
  40,
  24
)

-- Pro
insert into [Relay].[ServiceCatalogConfiguration]
(
  [ServiceCatalogId],
  [MaxConcurrentPlayers],
  [MaxPlayersPerGameServer],
  [GameStateUpdateChunkCount],
  [ClientRpcUpdateChunkCount],
  [BroadcastTickRate],
  [ReceiveTickRate]

)
values 
(
  3,
  500,
  16,
  128,
  64,
  40,
  24
)