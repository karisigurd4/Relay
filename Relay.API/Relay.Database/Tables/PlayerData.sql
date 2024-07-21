create table [Relay].[PlayerData]
(
  [Id] int identity(1, 1) not null,
  [PlayerId] int not null,
  [Key] nvarchar(256) not null,
  [Value] nvarchar(2048) default null,
  [Public] bit default 0,
  
  constraint PK_Relay_PlayerData primary key ([Id]),
  constraint FK_Relay_PlayerData_To_Player foreign key ([PlayerId]) references [Relay].[Player]([Id])
)