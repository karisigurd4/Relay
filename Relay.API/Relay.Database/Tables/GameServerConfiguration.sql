create table [Relay].[GameServerConfiguration]
(
  [Id] int identity(1, 1) not null,
  [GameServerId] int not null,
  [ServerName] nvarchar(256) not null,
  [Private] bit default 0,
  [Code] nvarchar(256),
  [Mode] nvarchar(256) default '',
  [MaxPlayers] int default 16,
    
  constraint PK_Relay_GameServerConfiguration_Id primary key ([Id]),
  constraint FK_Relay_GameServerConfiguration_To_GameServer_Id foreign key ([GameServerId]) references [Relay].[GameServer]([Id])
)