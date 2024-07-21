create procedure [Relay].[UpdatePlayer]
(
  @playerApiKey uniqueidentifier,
  @key nvarchar(256),
  @value nvarchar(2048),
  @publicData bit
)
as
begin
  declare @playerId int = 
  (
      select top 1 
          [Id] 
        from [Relay].[Player] 
          where [ApiKey] = @playerApiKey
  )

  if (@playerId is null) 
  begin
    select 0 as Success
    return
  end

  merge into [Relay].[PlayerData] as t
  using (
    values (
        @playerId, 
        @key, 
        @value,
        @publicData)
    ) as s ([PlayerId], [Key], [Value], [Public])
  on t.[PlayerId] = @playerId and t.[Key] = @key
  when matched then
    update set [Value] = @value
  when not matched by target then 
    insert ([PlayerId], [Key], [Value], [Public]) values (s.[PlayerId], s.[Key], s.[Value], s.[Public]);

  insert into [Relay].[PlayerActivity] 
  (
	  [PlayerId],
	  [ActivityType]
  )
  values
  (
	  @playerId,
	  'Updated'
  )

  select 1 as Success
end