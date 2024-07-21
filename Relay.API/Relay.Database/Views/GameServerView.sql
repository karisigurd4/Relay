create or alter view [Relay].[GameServerView]
as
  with serverPlayerCount as
  (
    select 
        [GameServerId], 
        COUNT(*) as [PlayerCount]
      from [Relay].GameServerPlayers 
        group by [GameServerId]
  )
  select 
      g.[Id] as [GameServerId],
      g.[State] as [GameServerState],
      g.[StartDate] as [GameServerStartDate],
      g.[ProjectId] as [GameServerProjectId],
      g.[IPAddress] as [GameServerIPAddress],
      g.[Port] as [GameServerPort],
      g.[GameServerHost] as [GameServerHost],
      g.[Tag] as [GameServerTag],
      spc.[PlayerCount] as [PlayerCount],
      ps.MaximumPlayerCapacity as [MaximumPlayerCapacity],
      datediff(day, pss.[LastCheckedStatusDate], getutcdate()) as [DaysSinceSubscriptionRefreshed],
      pss.[Active] as [SubscriptionActive]
    from [Relay].[GameServer] g
      left join serverPlayerCount spc
        on g.[Id] = spc.[GameServerId]
      left join [Relay].[ProjectSettings] ps
        on g.[ProjectId] = ps.[ProjectId]
      inner join Relay.ProjectSubscription pss
        on pss.[ProjectId] = g.[ProjectId]