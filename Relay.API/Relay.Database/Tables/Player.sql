create table [Relay].[Player]
(
  [Id] int identity(1, 1) not null,
  [ProjectId] uniqueidentifier not null,
  [Name] nvarchar(256) not null,
  [ApiKey] uniqueidentifier not null default newid(),
  [RegistrationDate] datetime2 not null default getutcdate(),
  
  constraint PK_Relay_Player primary key ([Id])
)