create table [Relay].[PlayerActivity]
(
  [Id] int identity(1, 1) not null,
  [PlayerId] int not null,
  [ActivityDateTime] datetime2 not null default getutcdate(),
  [ActivityType] nvarchar(256),
    
  constraint PK_Relay_PlayerActivity primary key ([Id]),
  constraint FK_Relay_PlayerActivity_To_Player foreign key ([PlayerId]) references [Relay].[Player]([Id])
)