create or alter procedure [Relay].[GetPlayerStatistics]
(
  @extAuthId nvarchar(256),
  @projectId uniqueidentifier,
  @period int
)
as
begin
  /*
    Period enum
      0 - By day
      1 - By month
      2 - By year
      3 - Total
  */
  if (@projectId = '00000000-0000-0000-0000-000000000000')
    set @projectId = null

  declare @sqlQuery nvarchar(max) = N'insert into #t
  select 
      g.[GameServerProjectId] as [ProjectId],
      ' + iif(@period > 2, 'null as [Year],', N'year(g.[RegistrationDate]) as [Year],') + N'
      ' + iif(@period > 1, 'null as [Month],', N'month(g.[RegistrationDate]) as [Month],') + N'
      ' + iif(@period > 0, 'null as [Day],', N'day(g.[RegistrationDate]) as [Day],') + N'
      count(g.[Id]) as [PlayerCount]
    from Relay.PlayerView g
      inner join Relay.ProjectSettings p
        on g.ProjectId = p.ProjectId
      where p.[ExtAuthId] = ''' + @extAuthId + N'''
        ' + iif(@projectId is not null, 'and g.[ProjectId] = ''' + convert(nvarchar(36), @projectId) + '''', '') + N'
    group by 
      g.[ProjectId]
      ' + iif(@period > 2, '', N', year(g.[RegistrationDate])') + N'
      ' + iif(@period > 1, '', N', month(g.[RegistrationDate])') + N'
      ' + iif(@period > 0, '', N', day(g.[RegistrationDate])')

  create table #t
  (
    [ProjectId] uniqueidentifier,
    [Year] int,
    [Month] int,
    [Day] int,
    [PlayerCount] int
  )

  EXECUTE sp_executesql @sqlQuery

  declare @jsonResults nvarchar(max) = (select * from #t for json path)

  select 1 as [Success], @jsonResults as [ResultsJson]
end