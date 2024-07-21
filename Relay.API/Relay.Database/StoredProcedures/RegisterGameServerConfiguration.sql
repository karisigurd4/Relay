create or alter procedure [Relay].[RegisterGameServerConfiguration]
(
  @gameServerId int,
  @serverName nvarchar(256),
  @isPrivate bit,
  @code nvarchar(256),
  @mode nvarchar(256),
  @maxPlayers int
)
as
begin
  insert into [Relay].[GameServerConfiguration]
  (
    [GameServerId],
    [ServerName],
    [Private],
    [Code],
    [Mode],
    [MaxPlayers]
  )
  values
  (
    @gameServerId,
    @serverName,
    @isPrivate,
    @code,
    @mode,
    @maxPlayers
  )

  select 1 as [Success]
end