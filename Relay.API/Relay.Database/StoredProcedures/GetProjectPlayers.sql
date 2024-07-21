create procedure [Relay].[GetProjectPlayers]
(
  @projectId uniqueidentifier,
  @count int,
  @offset int
)
as
begin
  declare @totalPlayers int = (select top 1 count(*) from Relay.PlayerView where [ProjectId] = @projectId)

  declare @playersJson nvarchar(max) = 
  (
    select 
        p.[Id],
        p.[Name],
        p.[RegistrationDate],
        p.[PublicPlayerDataJson],
        p.[PrivatePlayerDataJson],
        p.PlayerActive,
        p.[InGameServer], 
        p.[InGameServerId],
        p.[InGameServerIPAddress],
        p.[InGameServerPort]
      from [Relay].[PlayerView] p
        where p.[ProjectId] = @projectId
         order by p.[Name] desc
          offset @offset rows
          fetch next @count rows only
        for json path
  )

  select @playersJson as [PlayersJson], @totalPlayers as [TotalPlayerCount], 1 as Success
end