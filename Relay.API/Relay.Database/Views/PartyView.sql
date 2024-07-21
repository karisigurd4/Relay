create or alter view [Relay].[PartyView]
as
  select 
      pl.[PlayerId] as [PlayerId],
      pw.[Name] as [PlayerName],
      p.[Id] as [PartyId],
      iif(p.[PartyLeaderPlayerId] = pw.[Id], 1, 0) as [IsPartyLeader],
      pw.InGameServer as [InGameServer],
      pl.[LastPolledDateTime],
      iif(p.[PartyLeaderPlayerId] = pw.[Id], pw.[InGameServerId], null) as [InGameServerId],
      iif(p.[PartyLeaderPlayerId] = pw.[Id], pw.[InGameServerIPAddress], null) as [InGameServerIPAddress],
      iif(p.[PartyLeaderPlayerId] = pw.[Id], pw.[InGameServerPort], null) as [InGameServerPort]
    from [Relay].[Party] p
      inner join [Relay].[PartyPlayers] pl
        on p.[Id] = pl.[PartyId] and datediff(second, pl.[LastPolledDateTime], getutcdate()) < 30
      inner join [Relay].[PlayerView] pw 
        on pl.[PlayerId] = pw.[Id]
      
      
