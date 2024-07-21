create procedure [Relay].[AddNotificationMessage]
(
  @toPlayerId int,
  @referenceId int,
  @type nvarchar(256),
  @data nvarchar(1024)
)
as
begin
  insert into [Relay].[NotificationMessage] ([ToPlayerId], [ReferenceId], [Type], [Data]) values (@toPlayerId, @referenceId, @type, @data)

  select 1 as [Success]
end