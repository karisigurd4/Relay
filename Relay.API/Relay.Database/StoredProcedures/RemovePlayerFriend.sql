create procedure [Relay].[RemovePlayerFriend]
(
  @apiKey uniqueidentifier,
  @removePlayerId int
)
as
begin
  declare @playerId int = 
  (
    select
        [Id]
      from [Relay].[Player]
        where [ApiKey] = @apiKey
  )  

  delete from [Relay].[PlayerFriend] 
    where
      ([Player1Id] = @playerId and [Player2Id] = @removePlayerId)

  delete from [Relay].[PlayerFriend]
    where
      ([Player2Id] = @playerId and [Player1Id] = @removePlayerId)

  select 1 as [Success]
end