create procedure [Relay].[HideNotification]
(
  @id int
)
as
begin
  update [Relay].[NotificationMessage] set [HiddenFlag] = 1 where [Id] = @id

  select 1 as Success
end