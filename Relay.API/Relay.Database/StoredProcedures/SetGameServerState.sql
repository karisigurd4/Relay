create procedure [Relay].[SetGameServerState]
(
  @gameServerId int,
  @state nvarchar(32)
)
as
begin
  update [Relay].[GameServer] set [State] = @state where [Id] = @gameServerId

  select 1 as Success
end