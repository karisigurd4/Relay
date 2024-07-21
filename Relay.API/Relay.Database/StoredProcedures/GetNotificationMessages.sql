create procedure [Relay].[GetNotificationMessages]
(
  @apiKey uniqueidentifier,
  @offset int,
  @count int
)
as
begin
  select top 1 
      [Id] 
      into #playerId
    from [Relay].[Player] 
      where [ApiKey] = @apiKey

  select 
      Id
    into #selectedNotificationMessagesIds
      from [Relay].[NotificationMessage]
      where [HiddenFlag] = 0
        and [ToPlayerId] = (select top 1 [Id] from #playerId)
        order by [Id] desc
          offset @offset rows
          fetch next @count rows only  

    update [Relay].[NotificationMessage] set [ViewedFlag] = 1 where [Id] in (select [Id] from #selectedNotificationMessagesIds)

   declare @notificationMessagesJson nvarchar(max) = (
    select 
        [Id],
        [ToPlayerId],
        [ReferenceId],  
        [Type], 
        [SentDateTime],
        [Data],
        [ViewedFlag]
      from [Relay].[NotificationMessage]
        where Id in (select [Id] from #selectedNotificationMessagesIds)
        for json path
    )

    insert into [Relay].[PlayerActivity] 
    (
	    [PlayerId],
	    [ActivityType]
    )
    values
    (
	    (select top 1 [Id] from #playerId),
	    'GetNotifications'
    )

    select 1 as [Success], @notificationMessagesJson as [NotificationMessagesJson]
end