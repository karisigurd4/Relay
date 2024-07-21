create table [Relay].[GameServerOperationRequest]
(
  [Id] int identity(1, 1),
  [GameServerHost] nvarchar(256) not null,
  [RequestDateTime] datetime2 default getutcdate(),
  [Operation] nvarchar(256) not null,
  [RequestProjectId] uniqueidentifier not null,
  [RequestPort] int not null,
  [RequestGameServerId] int not null,
  
  constraint PK_Relay_StartGameServerRequest primary key ([Id]),
  constraint FK_Relay_GameServerOperationRequest foreign key ([GameServerHost]) references [Relay].[GameServerHost] ([HostName])
)
