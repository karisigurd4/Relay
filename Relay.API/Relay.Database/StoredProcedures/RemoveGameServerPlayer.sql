create procedure [Relay].[RemoveGameServerPlayer]
(
  @gameServerId int,
  @playerId int
)
as
begin
  delete from [Relay].[GameServerPlayers] where [GameServerId] = @gameServerId and [PlayerId] = @playerId

  select 1 as Success
end