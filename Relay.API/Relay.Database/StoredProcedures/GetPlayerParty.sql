create or alter procedure [Relay].[GetPlayerParty]
(
  @playerId int
)
as
begin
  declare @partyId int = 
  (
    select top 1
        [PartyId]   
      from [Relay].[PartyView]
        where [PlayerId] = @playerId  
  )

  update [Relay].[PartyPlayers] 
    set [LastPolledDateTime] = getutcdate() 
      where 
        [PartyId] = @partyId
        and [PlayerId] = @playerId

  declare @partyJson nvarchar(max) = 
  (
    select
      [PlayerId],
      [PlayerName],
      [PartyId],  
      [IsPartyLeader],  
      [InGameServer], 
      [InGameServerId],
      [InGameServerIPAddress],
      [InGameServerPort]
    from [Relay].[PartyView] 
      where [PartyId] = @partyId
      for json path
  )
  
  select 1 as [Success], @partyJson as [PartyJson]
end