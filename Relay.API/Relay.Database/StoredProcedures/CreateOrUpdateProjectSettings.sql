create or alter procedure [Relay].[CreateOrUpdateProjectSettings]
(
  @projectId uniqueidentifier, 
  @serviceCatalogId int,
  @lobbySystemType nvarchar(256),
  @gameModesJsonData nvarchar(max), 
  @maximumPlayerCapacity int,
  @enablePreGameLobby bit,
  @restrictJoiningToPreGameLobby bit,
  @maximumLobbyTimeInSeconds int,
  @enableMatchTimeLimit bit,
  @maximumActiveMatchTimeInSeconds int,
  @enableLevelBasedMatchmaking bit,
  @matchmakingPlayerDataKey nvarchar(256),
  @matchmakingOptimalRange int,
  @extAuthId nvarchar(256)
)
as
begin
  if (@serviceCatalogId = 0) 
  begin
    set @serviceCatalogId = 1
  end

  merge into [Relay].[ProjectSettings] as t
  using (
    values 
    (
      @extAuthId,
      @projectId,
      @serviceCatalogId,
      @gameModesJsonData,
      @lobbySystemType,
      @maximumPlayerCapacity,
      @enablePreGameLobby,
      @restrictJoiningToPreGameLobby,
      @maximumLobbyTimeInSeconds,
      @enableMatchTimeLimit,
      @maximumActiveMatchTimeInSeconds,
      @enableLevelBasedMatchmaking,
      @matchmakingPlayerDataKey,
      @matchmakingOptimalRange 
    )
  ) 
  as s 
  (
    [ExtAuthId],
    [ProjectId],
    [ServiceCatalogId],
    [LobbySystemType],
    [GameModesJsondata],
    [MaximumPlayerCapacity],
    [EnablePreGameLobby],
    [RestrictJoiningToPreGameLobby],
    [MaximumLobbyTimeInSeconds],
    [EnableMatchTimeLimit],
    [MaximumActiveMatchTimeInSeconds],
    [EnableLevelBasedMatchmaking],
    [MatchmakingPlayerDataKey],
    [MatchmakingOptimalRange] 
  )
    on t.ProjectId = @projectId and t.ExtAuthId = @extAuthId
    when matched then
      update set
        [ServiceCatalogId] = @serviceCatalogId,
        [LobbySystemType] = @lobbySystemType,
        [GameModesJsonData] = @gameModesJsonData,
        [MaximumPlayerCapacity] = @maximumPlayerCapacity,
        [EnablePreGameLobby] = @enablePreGameLobby,
        [RestrictJoiningToPreGameLobby] = @restrictJoiningToPreGameLobby,
        [MaximumLobbyTimeInSeconds] = @maximumLobbyTimeInSeconds,
        [EnableMatchTimeLimit] = @enableMatchTimeLimit,
        [MaximumActiveMatchTimeInSeconds] = @maximumActiveMatchTimeInSeconds,
        [EnableLevelBasedMatchmaking] = @enableLevelBasedMatchmaking,
        [MatchmakingPlayerDataKey] = @matchmakingPlayerDataKey,
        [MatchmakingOptimalRange] = @matchmakingOptimalRange
    when not matched by target then 
      insert 
      (
        [ExtAuthId],
        [ProjectId],
        [ServiceCatalogId],
        [GameModesJsonData],
        [LobbySystemType],
        [MaximumPlayerCapacity],
        [EnablePreGameLobby],
        [RestrictJoiningToPreGameLobby],
        [MaximumLobbyTimeInSeconds],
        [EnableMatchTimeLimit],
        [MaximumActiveMatchTimeInSeconds],
        [EnableLevelBasedMatchmaking],
        [MatchmakingPlayerDataKey],
        [MatchmakingOptimalRange]
      )
      values 
      (
        s.[ExtAuthId],
        s.[ProjectId],
        s.[ServiceCatalogId],
        s.[LobbySystemType],
        s.[GameModesJsonData],
        s.[MaximumPlayerCapacity],
        s.[EnablePreGameLobby],
        s.[RestrictJoiningToPreGameLobby],
        s.[MaximumLobbyTimeInSeconds],
        s.[EnableMatchTimeLimit],
        s.[MaximumActiveMatchTimeInSeconds],
        s.[EnableLevelBasedMatchmaking],
        s.[MatchmakingPlayerDataKey],
        s.[MatchmakingOptimalRange]
      );

  select 1 as Success
end