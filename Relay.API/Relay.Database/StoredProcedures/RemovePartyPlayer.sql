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
      ([Player1Id] = @playerId or [Player2Id] = @removePlayerId)
      and
      ([Player2Id] = @playerId or [Player1Id] = @removePlayerId)

  insert into [Relay].[PlayerActivity] 
  (
	  [PlayerId],
	  [ActivityType]
  )
  values
  (
	  @playerId,
	  'RemovedFriend'
  )

  insert into [Relay].[PlayerActivity] 
  (
	  [PlayerId],
	  [ActivityType]
  )
  values
  (
	  @removePlayerId,
	  'RemovedAsFriend'
  )

  select 1 as [Success]
end