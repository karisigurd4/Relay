create procedure [Relay].[GetUnreadNotificationMessagesCount]
(
  @playerApiKey uniqueidentifier
)
as
begin
  select top 1 
      [Id] 
      into #playerId
    from [Relay].[Player] 
      where [ApiKey] = @playerApiKey

  select 1 as [Success], count(*)  as [Count]
    from [Relay].[NotificationMessage] as [Count] 
      where 
        [ToPlayerId] = (select top 1 [Id] from #playerId) 
        and [ViewedFlag] = 0 and [HiddenFlag] = 0
end