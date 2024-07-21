create procedure [Relay].[AddGameServer]
(
  @hostName nvarchar(256),
  @ipAddress nvarchar(256),
  @port int,
  @projectId uniqueidentifier,
  @tag nvarchar(256)
)
as
begin
  declare @idTbl as table ([Id] int)

  insert into [Relay].[GameServer] ([GameServerHost], [IPAddress], [Port], [ProjectId], [Tag]) 
      output inserted.Id into @idTbl
    values (@hostName, @ipAddress, @port, @projectId, @tag)

  select 1 as Success, (select top 1 [Id] from @idTbl) as [GameServerId]
end