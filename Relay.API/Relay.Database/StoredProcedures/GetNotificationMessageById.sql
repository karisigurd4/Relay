create procedure [Relay].[GetNotificationMessageById]
(
  @id int
)
as
begin
  select
      [Id],
      [ToPlayerId],
      [ReferenceId],
      [Type],   
      [Data],
      [SentDateTime]
    from [Relay].[NotificationMessage]
      where [Id] = @id
end