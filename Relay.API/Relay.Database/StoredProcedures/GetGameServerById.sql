create procedure [Relay].[GetGameServerById]
(
  @gameServerId int
)
as
begin
  declare @gameServerJson nvarchar(max) = 
  (
  select
      [Id], 
      [ProcessId],    
      [IPAddress],  
      [Port], 
      [StartDate],
      [State]
    from [Relay].[GameServer]
      Where [Id] = @gameServerId
      for json path, without_array_wrapper
  )

  select 1 as [Success], @gameServerJson as [GameServerJson]
end