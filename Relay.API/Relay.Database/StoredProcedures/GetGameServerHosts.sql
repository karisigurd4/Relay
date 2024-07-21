create procedure [Relay].[GetGameServerHosts]
as
begin
  declare @hostsJson nvarchar(max) =
  (
    select 
        [HostName],
        [CpuUsage],   
        [MemoryUsage],
        [Status],
        [ActiveGameServerProcessCount]
      from [Relay].[GameServerHostView]
      for json path
  ) 

  select 1 as [Success], @hostsJson as [HostsJson]
end