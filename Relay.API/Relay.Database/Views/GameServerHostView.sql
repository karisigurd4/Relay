create or alter view [Relay].[GameServerHostView]
as
  select 
    h.[HostName],
    h.[CpuUsage],
    h.[MemoryUsage],
    count(gs.Id) as [ActiveGameServerProcessCount],
    iif(datediff(second, h.[LastUpdate], getutcdate()) > 10, 'Down', 'Up') as [Status]
  from [Relay].[GameServerHost] h
    left join [Relay].[GameServer] gs
      on 
        gs.[GameServerHost] = h.[HostName] and 
        gs.[State] != 'Finished'
    group by h.[HostName], h.[CpuUsage], h.[MemoryUsage], h.[LastUpdate]