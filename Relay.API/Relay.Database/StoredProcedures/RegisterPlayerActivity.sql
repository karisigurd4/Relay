create procedure [Relay].[RegisterPlayerActivity]
(
  @playerId int,
  @activityType nvarchar(256)
)
as
begin
  insert into [Relay].[PlayerActivity] ([PlayerId], [ActivityType]) values (@playerId, @activityType)
end
go