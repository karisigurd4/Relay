create procedure [Relay].[GetGameServerInfoById]
(
	@gameServerId int
)
as
begin
declare @playersJson nvarchar(max) = 
(
	select 
			p.[Id] as [PlayerId],
			p.[Name] as [PlayerName]
		from [Relay].[Player] p
		inner join [Relay].[GameServerPlayers] gp
			on p.Id = gp.PlayerId
		where GameServerId = @gameServerId
		for json path
)

declare @playerCount int = 
(
	select count(*)
		from [Relay].[GameServerPlayers] 
		where GameServerId = @gameServerId
)

select
		1 as [Success],
		g.[Id],		
		g.[IPAddress],
		g.[Port],	
		g.[StartDate],		
		g.[StopDate],
		g.[State],
		@playersJson as [PlayersJson],
		@playerCount as [PlayerCount]
	from [Relay].[GameServer] g
		where Id = @gameServerId
end