create or alter procedure [Relay].[GetProjectSubscription]
(
  @projectId uniqueidentifier
)
as
begin
  declare @projectSubscriptionJson nvarchar(max) = 
  (
    select top 1
        [ProjectId],
        [SubscriptionId],   
        [Active],
        [LastCheckedStatusDate],
        [CheckExpirationFlag]
      from [Relay].[ProjectSubscription]
        where [ProjectId] = @projectId
        for json path, without_array_wrapper
  )
  
  if (nullif(nullif(@projectSubscriptionJson, '{}'), '') is null) 
  begin
    select 0 as [Success] 
    return
  end

  select 1 as [Success], @projectSubscriptionJson as [ProjectSubscriptionJson]
end