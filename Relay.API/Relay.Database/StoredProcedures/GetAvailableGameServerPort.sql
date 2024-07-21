create or alter procedure [Relay].[GetAvailableGameServerPort]
(
  @gameServerHost nvarchar(256)
)
as
begin
	;with generatedNumbers as 
	(
		select 11000 as PortNumber
		union all
		select PortNumber+1 from generatedNumbers where PortNumber < 11999
	),
	activeGameServers as
	(
		select 
        [GameServerHost],
				[Port]
			from [Relay].[GameServer]
				where [State] != 'Finished'
	)
	select top 1
			PortNumber 
		from generatedNumbers
		  where [PortNumber] not in (select [Port] from activeGameServers where [GameServerHost] = @gameServerHost)
		  option (maxrecursion 10000)
end
