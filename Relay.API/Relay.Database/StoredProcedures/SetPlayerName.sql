create procedure [Relay].[SetPlayerName]
(
  @playerApiKey uniqueidentifier,
  @newPlayerName nvarchar(256)
)
as
begin
  update [Relay].[Player] set [Name] = @newPlayerName where [ApiKey] = @playerApiKey

  select 1 as [Success]
end