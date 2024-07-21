create procedure [Relay].[GetGameServerByCode]
(
  @projectId uniqueidentifier,
  @code nvarchar(32)
)
as
begin
  declare @gameServerJson nvarchar(max) = 
  (
    select top 1
        g.[GameServerId]as [GameServerId],
        g.[GameServerIPAddress] as [IPAddress],
        g.[GameServerPort] as [Port],
        c.[ServerName] as [GameServerName],
        c.[Private] as [Private],
        c.[Mode] as [Mode],
        c.[MaxPlayers] as [MaxPlayerCapacity],
        g.[PlayerCount] as [CurrentPlayerCount],
        g.[GameServerState] as [GameServerState]
      from [Relay].[GameServerView] g
        inner join [Relay].[GameServerConfiguration] c
          on g.[GameServerId] = c.[GameServerId]
        where g.[GameServerProjectId] = @projectId 
          and c.[Code] = @code
          for json path, without_array_wrapper
  )

  if (nullif(nullif(@gameServerJson, '{}'), '') is null)
  begin
    select 0 as [Success]
    return
  end

  select 1 as [Success], @gameServerJson as [ResultJson]
end