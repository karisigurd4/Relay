create table [Relay].[NotificationMessage]
(
  [Id] int identity(1, 1) not null,
  [ReferenceId] int default null,
  [ToPlayerId] int not null,
  [Type] nvarchar(256) not null,
  [Data] nvarchar(2048) default '',
  [ViewedFlag] bit default 0,
  [HiddenFlag] bit default 0,
  [SentDateTime] datetime2 default getutcdate(),

  constraint PK_Relay_NotificationMessage_Id primary key ([Id]),
  constraint FK_Relay_NotificationMessage_To_Player foreign key ([ToPlayerId]) references [Relay].[Player]([Id])
)