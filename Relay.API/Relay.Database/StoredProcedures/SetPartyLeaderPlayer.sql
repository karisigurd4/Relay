create procedure [Relay].[SetPartyLeaderPlayer]
(
  @leaderPlayerApiKey uniqueidentifier,
  @partyId int,
  @newLeaderPlayerId int
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

  update [Relay].[Party] set [PartyLeaderPlayerId] = @partyId where [Id] = @newLeaderPlayerId

  select 1 as Success
end
go
