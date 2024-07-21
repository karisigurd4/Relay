create table [Relay].[GameServer]
(
  [Id] int identity(1, 1) not null,
  [GameServerHost] nvarchar(256) default '',
  [ProjectId] uniqueidentifier not null,
  [State] nvarchar(32) not null default 'Lobby',
  [StartDate] datetime2 default getutcdate(),
  [StopDate] datetime2 default null,
  [ProcessId] int default null,
  [IPAddress] nvarchar(256) not null,
  [Port] int not null,
  [Tag] nvarchar(256) default null,
  
  constraint PK_Relay_GameServer primary key ([Id]),
  constraint FK_Relay_GameServer_To_GameServerHost foreign key ([GameServerHost]) references [Relay].[GameServerHost] ([HostName]),
  unique clustered ([ProjectId], [Id])
)