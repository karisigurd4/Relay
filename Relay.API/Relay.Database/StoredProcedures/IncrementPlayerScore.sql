create procedure [Relay].[IncrementPlayerScore]
(
  @gameServerId int,
  @playerId int,
  @hashedScoreType int,
  @scoreType nvarchar(256),
  @amount int
)
as
begin
  merge into [Relay].[PlayerScore] as t
    using (values
    (
      @gameServerId,
      @playerId,
      @hashedScoreType,
      @scoreType,
      @amount)
    ) as s ([GameServerId],[PlayerId],[HashedScoreType],[ScoreType],[Amount])
  on t.[GameServerId] = @gameServerId and t.[PlayerId] = @playerId
  when matched then 
    update set [Value] = [Value] + s.[Amount]
  when not matched by target then
    insert ([GameServerId], [PlayerId], [HashedScoreType], [ScoreType], [Value]) 
      values (s.[GameServerId], s.[PlayerId], s.[HashedScoreType], s.[ScoreType], s.[Amount]);

  select 1 as Success
end