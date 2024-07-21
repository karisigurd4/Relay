create or alter procedure [Relay].[GetPlayer]
(
	@projectId uniqueidentifier,
  @playerId int,
  @playerApiKey uniqueidentifier
)
as
begin
	declare @localPlayerId int = isnull(@playerId, (select top 1 [Id] from [Relay].[Player] where [ApiKey] = @playerApiKey))

	if (@localPlayerId is null) 
	begin
		select 0 as Success, null as [PlayerJson]
		return
	end

	if ((select [ProjectId] from [Relay].[Player] where [Id] = @localPlayerId and [ProjectId] = @projectId) is null) 
	begin
		select 0 as Success, null as [PlayerJson]
		return
	end

	declare @playerJson nvarchar(max) = (select 
		p.[Id],
		iif(@localPlayerId is null, null, p.[ApiKey]) as [ApiKey],
		p.[RegistrationDate],
		p.[Name],
		p.PlayerActive
	from [Relay].[PlayerView] as p
		where p.[Id] = @localPlayerId
		for json path, without_array_wrapper
	)

	declare @publicPlayerDataJson nvarchar(max) =
	(
		select 
				[Key],
				[Value]
			from [Relay].[PlayerData] 
				where [PlayerId] = @localPlayerId
				and [Public] = 1
				for json path
	)	

	declare @privatePlayerDataJson nvarchar(max) 
	if (@playerApiKey is not null)
	begin
		set @privatePlayerDataJson =
		(
				select 
						[Key],
						[Value]
					from [Relay].[PlayerData] 
						where [PlayerId] = @localPlayerId
						and [Public] = 0
						for json path
			)
	end

	if (@playerJson is null or @playerJson = '{}') 
	begin
		select 0 as Success, null as [PlayerJson]
		return
	end

	if (@playerApiKey is not null) 
	begin
		insert into [Relay].[PlayerActivity] 
    (
	    [PlayerId],
	    [ActivityType]
    )
    values
    (
	    @localPlayerId,
	    'GetPlayerByApiKey'
    )
	end

	select 1 as Success, @playerJson as [PlayerJson], @publicPlayerDataJson as [PublicPlayerDataJson], @privatePlayerDataJson as [PrivatePlayerDataJson]
end