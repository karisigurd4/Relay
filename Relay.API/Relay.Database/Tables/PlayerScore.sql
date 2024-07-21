create table [Relay].[PlayerScore] 
(
  [Id] int identity(1, 1) not null,
  [GameServerId] int not null,
  [PlayerId] int not null,
  [HashedScoreType] int not null,
  [ScoreType] nvarchar(256) not null,
  [Value] int not null,

  constraint PK_Relay_PlayerScore_Id primary key([Id]),
  constraint FK_Relay_PlayerScore_GameServerId_to_GameServer foreign key ([GameServerId]) references [Relay].[GameServer]([Id]),
  constraint FK_Relay_PlayerScore_PlayerId_To_Player foreign key ([PlayerId]) references [Relay].[Player]([Id]),
  index IX_Relay_PlayerScore_HashedScoreType nonclustered([HashedScoreType])
)