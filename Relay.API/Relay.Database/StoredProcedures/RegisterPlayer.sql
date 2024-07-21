create procedure [Relay].[RegisterPlayer]
(
  @name nvarchar (256),
  @projectId uniqueidentifier
)
as
begin
  declare @apiKey table ([Id] int, [ApiKey] uniqueidentifier)

  insert into [Relay].[Player] ([ProjectId], [Name]) 
    output inserted.Id, inserted.ApiKey into @apiKey
    values (@projectId, @name)

	insert into [Relay].[PlayerActivity] 
  (
	  [PlayerId],
	  [ActivityType]
  )
  values
  (
	  (select top 1 [Id] from @apiKey),
	  'Registered'
  )

  select 1 as [Success], [Id], [ApiKey] from @apiKey
end