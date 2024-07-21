create procedure [Relay].[GetServiceCatalogConfiguration]
(
  @projectId uniqueidentifier
)
as
begin
  declare @serviceCatalogId int = (
    select top 1 
        [ServiceCataLogId] 
      from [Relay].[ProjectSettings]
        where [ProjectId] = @projectId
  )

  declare @json nvarchar(max) = (
	select
      [ServiceCatalogId],
      [MaxConcurrentPlayers],
      [MaxPlayersPerGameServer],
      [GameStateUpdateChunkCount],
      [ClientRpcUpdateChunkCount],
      [BroadcastTickRate],
      [ReceiveTickRate]
    from [Relay].[ServiceCatalogConfiguration]
      where [ServiceCatalogId] = @serviceCatalogId
	for json path, without_array_wrapper
  )

  select 1 as [Success], @json as [ServiceCatalogConfigurationJson]
end