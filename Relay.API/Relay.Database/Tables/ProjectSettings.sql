create table [Relay].[ProjectSettings]
(
  [ProjectId] uniqueidentifier not null,
  [ServiceCatalogId] int default 1,
  [LobbySystemType] nvarchar(256) default 'Matchmaking',
  [GameModesJsonData] nvarchar(max) default '[]',
  [MaximumPlayerCapacity] int default 16,
  [EnablePreGameLobby] bit default 1,
  [RestrictJoiningToPreGameLobby] bit default 1,
  [MaximumLobbyTimeInSeconds] int default 30,
  [EnableMatchTimeLimit] bit default 1,
  [MaximumActiveMatchTimeInSeconds] int default 180,
  [EnableLevelBasedMatchmaking] bit default 0,
  [MatchmakingPlayerDataKey] nvarchar(256) default null,
  [MatchmakingOptimalRange] int default null,
  [ExtAuthId] nvarchar(256),

  constraint PK_Relay_ProjectSettings_Id primary key ([ProjectId])
)
