create or alter procedure [Relay].[SearchPlayers]
(
  @projectId uniqueidentifier,
  @query nvarchar(256),
  @offset int,
  @count int
)
as
begin
  select 
      [Id],
      [Name],  
      [PlayerActive]
      into #searchPlayerResults
    from [Relay].[PlayerView]
    where 
      [Name] like concat(@query, '%') and
      [ProjectId] = @projectId
    
     
  declare @searchPlayersResultsJson nvarchar(max) = 
  (
    select 
        [Id], 
        [Name], 
        [PlayerActive] 
      from #searchPlayerResults 
      for json path
  )

  declare @totalMatches int = (select count(*) from #searchPlayerResults)
    
  select 1 as [Success], @searchPlayersResultsJson as [PlayersJson], @totalMatches as [TotalMatches]
end