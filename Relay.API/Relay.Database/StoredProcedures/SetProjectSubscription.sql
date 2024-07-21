create procedure [Relay].[SetProjectSubscription]
(
  @projectId uniqueidentifier,
  @subscriptionId nvarchar(256),
  @active bit
)
as
begin
  merge into [Relay].[ProjectSubscription] as t
  using (
    values (
      @projectId,
      @subscriptionId,
      @active
    )
  )
  as s
  (
    [ProjectId],
    [SubscriptionId],
    [Active]
  )
    on t.[ProjectId] = @projectId
    when matched then
      update set
        [SubscriptionId] = @subscriptionId,
        [Active] = @active,
        [LastCheckedStatusDate] = getutcdate()
    when not matched by target then
      insert 
      (
        [ProjectId],
        [SubscriptionId],
        [Active]
      )
      values
      (
        s.[ProjectId],
        s.[SubscriptionId],
        s.[Active]
      );

  select 1 as [Success];
end