create procedure [Relay].[GetPlayerFriendsList]
(
  @apiKey uniqueidentifier
)
as
begin
  declare @playerId int = 
  (
    select top 1 [Id]
      from [Relay].[Player]
        where [ApiKey] = @apiKey
  )

  declare @friendIdTbl table
  (
    [Id] int
  )  

  insert into @friendIdTbl ([Id]) 
  (
    select distinct 
		iif([Player1Id] = @playerId, [Player2Id], [Player1Id]) as [Id]
      from [Relay].[PlayerFriend]
        where [Player1Id] = @playerId
        or [Player2Id] = @playerId
  )

  declare @friendListJson nvarchar(max) = 
  (
    select 
		p.[Id],
		p.[RegistrationDate],
		p.[Name],
		p.PlayerActive
	  from [Relay].[PlayerView] as p
		  where p.[Id] in (select [Id] from @friendIdTbl)
		  for json path
  )

		insert into [Relay].[PlayerActivity] 
    (
	    [PlayerId],
	    [ActivityType]
    )
    values
    (
	    @playerId,
	    'GetFriendList'
    )

  select 1 as [Success], @friendListJson as [PlayersJson]
end