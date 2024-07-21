create procedure [Relay].[SetProjectServiceTier]
(
  @projectId uniqueidentifier,
  @serviceTier int
)
as
begin
  update [Relay].[ProjectSettings] set [ServiceCatalogId] = @serviceTier
    where [ProjectId] = @projectId

  select 1 as [Success]
end