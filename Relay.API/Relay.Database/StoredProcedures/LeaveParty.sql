create procedure [Relay].[LeaveParty]
(
  @playerApiKey uniqueidentifier
)
as
begin
  declare @playerId int = 
  (
    select [Id] from [Relay].[Player] where [ApiKey] = @playerApiKey
  )

  delete from [Relay].[PartyPlayers] where [PlayerId] = @playerId

  select 1 as [Success]
end