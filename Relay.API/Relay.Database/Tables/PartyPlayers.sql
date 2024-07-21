create table [Relay].[PartyPlayers]
(
  [PlayerId] int not null,
  [PartyId] int not null,
  [LastPolledDateTime] datetime2 default getutcdate(),
  
  constraint PK_Relay_PartyPlayers primary key ([PlayerId], [PartyId]),
  constraint FK_Relay_PartyPlayers_To_Player foreign key ([PlayerId]) references [Relay].[Player]([Id]),
  constraint FK_Relay_PartyPlayers_To_Party foreign key ([PartyId]) references [Relay].[Party]([Id])
)