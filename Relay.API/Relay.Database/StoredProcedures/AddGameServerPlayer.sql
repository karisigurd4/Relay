create procedure [Relay].[AddGameServerPlayer]
(
  @gameServerId int,
  @playerId int
)
as
begin
  insert into [Relay].[GameServerPlayers] ([GameServerId], [PlayerId]) values (@gameServerId, @playerId)

  select 1 as Success
end