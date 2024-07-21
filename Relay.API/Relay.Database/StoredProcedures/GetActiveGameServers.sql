create procedure [Relay].[GetActiveGameServers]
(
  @hostName nvarchar(256)
)
as
begin
  declare @gameServersJson nvarchar(max) = 
  (
    select
        [Id], 
        [ProcessId],    
        [IPAddress],  
        [Port], 
        [StartDate],
        [State]
      from [Relay].[GameServer]   
        where [State] != 'Finished'
        and [GameServerHost] = @hostName
        for json path
  )

  select 1 as Success, @gameServersJson as [GameServersJson]
end