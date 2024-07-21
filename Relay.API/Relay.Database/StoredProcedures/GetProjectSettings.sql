create or alter procedure [Relay].[GetProjectSettings]
(
  @projectId uniqueidentifier
)
as
begin
  if ((select [ProjectId] from [Relay].[ProjectSettings] where [ProjectId] = @projectId) is null)
  begin
    select 0 as Success
    return
  end

  declare @projectSettingsJson nvarchar(max) = 
  (
    select
      ps.[LobbySystemType],
      IIF(ps.MaximumPlayerCapacity <= sc.MaxPlayersPerGameServer, ps.MaximumPlayerCapacity, sc.MaxPlayersPerGameServer) as [MaximumPlayerCapacity],
      ps.[EnablePreGameLobby],
      ps.[RestrictJoiningToPreGameLobby],
      ps.[MaximumLobbyTimeInSeconds],
      ps.[EnableMatchTimeLimit],
      ps.[MaximumActiveMatchTimeInSeconds],
      ps.[EnableLevelBasedMatchmaking],
      ps.[MatchmakingPlayerDataKey],
      ps.[MatchmakingOptimalRange]
    from [Relay].[ProjectSettings] ps
      inner join [Relay].ServiceCatalogConfiguration sc
        on ps.ServiceCatalogId = sc.ServiceCatalogId
      where ps.[ProjectId] = @projectId
      for json path, without_array_wrapper
  )

  select 1 as Success, @projectSettingsJson as [ProjectSettingsJson]
end