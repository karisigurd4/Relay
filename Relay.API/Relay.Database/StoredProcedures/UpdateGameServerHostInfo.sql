create procedure [Relay].[UpdateGameServerHostInfo] 
(
  @hostName nvarchar(256),
  @cpuUsage float,
  @memoryUsage float
)
as
begin
  merge into [Relay].[GameServerHost] as t
  using (
    values (
      @hostName,
      @cpuUsage,
      @memoryUsage
    )
  ) as s ([HostName], [CpuUsage], [MemoryUsage])
  on t.[HostName] = @hostName
  when matched then
    update set 
      [CpuUsage] = @cpuUsage,
      [MemoryUsage] = @memoryUsage,
      [LastUpdate] = getutcdate()
  when not matched by target then
    insert ([HostName], [CpuUsage], [MemoryUsage]) values (s.[HostName], s.[CpuUsage], s.[MemoryUsage]);

  select 1 as [Success]
end