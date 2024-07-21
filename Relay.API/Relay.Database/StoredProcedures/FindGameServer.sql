create procedure [Relay].[FindGameServer]
(
  @projectId uniqueidentifier,
	@tag nvarchar(256)
)
as
begin
	declare @serverId int = 
	(
		select top 1
				gs.[GameServerId] as [Id]
			from [Relay].[GameServerView] gs
				inner join [Relay].[ProjectSettings] ps
					on gs.[GameServerProjectId] = ps.ProjectId
				where 
				gs.[GameServerProjectId] = @projectId and 
				gs.[GameServerTag] = @tag and
				case 
					when (ps.EnablePreGameLobby = 1 and ps.RestrictJoiningToPreGameLobby = 1) then iif(gs.[GameServerState] = 'Lobby', 1, 0)
					else iif(gs.[GameServerState] != 'Finished', 1, 0) 
				end = 1 and
        isnull(gs.[PlayerCount], 0) < ps.[MaximumPlayerCapacity]
				order by gs.[GameServerId] asc
	)

	if (@serverId is null)
	begin
		select 1 as Success, null as GameServerJson
		return
	end

	declare @playerCount int = (select top 1 [PlayerCount] from [Relay].[GameServerView] where [GameServerId] = @serverId)

	declare @maxPlayerCount int = 
	(
		select iif(ps.MaximumPlayerCapacity <= scc.[MaxPlayersPerGameServer], ps.MaximumPlayerCapacity, scc.[MaxPlayersPerGameServer])
			from [Relay].[ProjectSettings] ps
				inner join [Relay].[ServiceCatalogConfiguration] scc
					on ps.ServiceCatalogId = scc.[ServiceCatalogId]
			where ProjectId = @projectId
	)  

	if (@playerCount >= @maxPlayerCount)
	begin
		select 1 as Success, null as GameServerJson
		return
	end

	declare @gameServerJson nvarchar(max) = 
  (
    select 
          gs.[Id],
          gs.[IpAddress],    
          gs.[Port], 
          gs.[StartDate]
        from [Relay].[GameServer] gs
				where gs.[Id] = @serverId
      for json path, without_array_wrapper
  )

  select 1 as [Success], @gameServerJson as [GameServerJson]
end