create procedure [Relay].[SetGameServerProcessId]
(
  @gameServerId int,
  @processId int
)
as
begin
  update [Relay].[GameServer] set [ProcessId] = @processId where [Id] = @gameServerId

  select 1 as [Success]
end