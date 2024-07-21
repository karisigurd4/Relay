create or alter procedure [Relay].[AddGameServerOperationRequest]
(
  @gameServerHost nvarchar(256),
  @operation nvarchar(256),
  @port int,
  @gameServerId int,
  @projectId uniqueidentifier
)
as
begin
  insert into [Relay].[GameServerOperationRequest]
  (
    [GameServerHost],
    [Operation],
    [RequestPort],
    [RequestGameServerId],
    [RequestProjectId]
  )
  values
  (
    @gameServerHost,
    @operation,
    @port,
    @gameServerId,
    @projectId
  )

  select 1 as [Success]
end