create table [Relay].[PlayerFriend]
(
  [Player1Id] int not null,
  [Player2Id] int not null,

  constraint PK_PlayerFriend_Id primary key ([Player1Id], [Player2Id])
)