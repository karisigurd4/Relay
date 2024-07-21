create procedure [Relay].[AddPartyPlayer]
(
  @partyId int,
  @playerId int
)
as
begin
  insert into [Relay].[PartyPlayers] ([PartyId], [PlayerId]) values (@partyId, @playerId)

  select 1 as [Success]
end