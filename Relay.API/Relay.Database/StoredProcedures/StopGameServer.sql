create procedure [Relay].[StopGameServer]
(
	@gameServerId int
)
as
begin
	update [Relay].[GameServer] set [State] = 'Finished' where [Id] = @gameServerId
end