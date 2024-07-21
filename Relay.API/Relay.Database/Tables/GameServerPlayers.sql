create table [Relay].[GameServerPlayers]
(
  [Id] int identity(1, 1) not null,
  [GameServerId] int,
  [PlayerId] int,
    
  constraint PK_GameServerPlayers primary key ([Id]),
  constraint FK_Relay_GameServerPlayers_To_GameServer foreign key ([GameServerId]) references [Relay].[GameServer]([Id]),
  constraint FK_Relay_GameServerPlayers_To_Player foreign key ([PlayerId]) references [Relay].[Player]([Id])
)