create procedure [Relay].[CreateParty]
(
  @partyLeaderPlayerId int
)
as
begin
  declare @idTbl table ([Id] int)

  insert into [Relay].[Party] ([PartyLeaderPlayerId]) 
	output Inserted.Id into @idTbl
	values (@partyLeaderPlayerId)

  insert into [Relay].[PartyPlayers] 
  ( 
	  [PartyId],
	  [PlayerId]
  )
  values
  (
	  (select top 1 [Id] from @idTbl),
	  @partyLeaderPlayerId
  )

  select 1 as [Success]
end