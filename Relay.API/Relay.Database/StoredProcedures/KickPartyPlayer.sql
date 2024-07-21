create procedure [Relay].[KickPartyPlayer]
(
  @leaderPlayerApiKey uniqueidentifier,
  @partyId int,
  @kickPlayerId int
)
as
begin
  declare @playerId int = 
  (
    select 
        [Id]
      from [Relay].[Player] 
        where [ApiKey] = @leaderPlayerApiKey
  )
  

  declare @partyIdLeaderPlayerId int = 
  (
    select
        pa.[PartyLeaderPlayerId]
      from [Relay].[Party] pa
        where pa.[Id] = @partyId
  )

  if (@playerId != @partyIdLeaderPlayerId)
  begin
    select 0 as Success
    return
  end

  exec [Relay].[RemovePartyPlayer] @partyId, @kickPlayerId
end