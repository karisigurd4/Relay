create or alter procedure [Relay].[AddPlayerFriend]
(
  @player1Id int,
  @player2Id int
)
as
begin
  insert into [Relay].[PlayerFriend] ([Player1Id], [Player2Id]) values (@player1Id, @player2Id)

  select 1 as [Success]
end