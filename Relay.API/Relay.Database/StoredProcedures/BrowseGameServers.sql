create or alter procedure [Relay].[BrowseGameServers]
(
  @projectId uniqueidentifier,
  @hideFull bit,
  @hidePrivate bit,
  @page int,
  @pageSize int,
  @orderBy nvarchar(32),
  @orderDirection nvarchar(4)
)
as
begin
  declare @gameServerListJson nvarchar(max) = 
  (
    select 
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
        where g.[GameServerProjectId] = @projectId and
          g.[GameServerState] != 'Finished' and
          case when @hideFull = 1
            then IIF(g.[PlayerCount] >= c.[MaxPlayers], 0, 1)
            else 1
          end = 1 and 
          case when @hidePrivate = 1
            then iif(c.[Private] = 1, 0, 1)
            else 1
          end = 1 
          order by
            CASE 
              WHEN @orderBy = 'ServerName' AND @OrderDirection = 'asc' THEN ServerName
            END ASC,
            CASE
              WHEN @orderBy = 'ServerName' AND @OrderDirection = 'desc' THEN ServerName
            END DESC,
            CASE
              WHEN @orderBy = 'PlayerCount' AND @OrderDirection = 'asc' THEN PlayerCount
            END ASC,
            CASE
              WHEN @orderBy = 'PlayerCount' AND @OrderDirection = 'desc' THEN PlayerCount
            END DESC
      offset @page * @pageSize rows
        fetch next @pageSize rows only
        for json path
  )

  declare @totalCount int = 
  (
   select top 1
        count(g.[GameServerId]) 
      from [Relay].[GameServerView] g
        inner join [Relay].[GameServerConfiguration] c
          on g.[GameServerId] = c.[GameServerId]
        where g.[GameServerProjectId] = @projectId and
          g.[GameServerState] != 'Finished' and
          case when @hideFull = 1
            then IIF(g.[PlayerCount] >= c.[MaxPlayers], 0, 1)
            else 1
          end = 1 and 
          case when @hidePrivate = 1
            then iif(c.[Private] = 1, 0, 1)
            else 1
          end = 1 
  )

 select 1 as [Success], @gameServerListJson as GameServerListJson, @totalCount as [TotalCount]
end