create table [Relay].[Party]
(
  [Id] int identity(1, 1) not null,
  [Status] nvarchar(256) default 'Waiting',
  [PartyLeaderPlayerId] int,
  [ActiveGameServerId] int,

  constraint PK_Relay_Party_Id primary key ([Id]),
  constraint FK_Relay_Party_To_GameServer foreign key ([ActiveGameServerId]) references [Relay].[GameServer]([Id])
)