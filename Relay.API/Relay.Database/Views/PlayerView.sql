create or alter view [Relay].[PlayerView]
as
  with PlayerDataKeyValueJson as
  (
    select [PlayerId], [Public], concat('"', [Key], '": ', '"', [Value], '"')
      as JsonPlayerData from [Relay].[PlayerData]
  ),
  AggregatedJson as
  (
    select [PlayerId], [Public], concat('{', string_agg(JsonPlayerData, ', '), '}') as PlayerDataJson
      from PlayerDataKeyValueJson
        group by [PlayerId], [Public]
  ),
  InGamePlayers as
  (
    select
        gp.[PlayerId] as [PlayerId],
        gs.[Id] as [GameServerId],
        gs.[IPAddress] as [GameServerIPAddress],
        gs.[Port] as [GameServerPort]
      from [Relay].[GameServerPlayers] gp
        inner join [Relay].[GameServer] gs
          on gp.[GameServerId] = gs.[Id]
        where gs.[State] != 'Finished'
  )
  select 
      p.[Id],
      p.[ApiKey],
      p.[Name],
      p.[RegistrationDate],
      p.[ProjectId],
      jpu.PlayerDataJson as PublicPlayerDataJson,
      jpr.PlayerDataJson as PrivatePlayerDataJson,
      isnull(pa.[PlayerActive], 0) as [PlayerActive],
      iif(igp.[PlayerId] is null, 0, 1) as [InGameServer],
      igp.[GameServerId] as [InGameServerId],
      igp.[GameServerIPAddress] as [InGameServerIPAddress],
      igp.[GameServerPort] as [InGameServerPort]
    from [Relay].[Player] p
      left join AggregatedJson jpu
        on p.[Id] = jpu.[PlayerId] and jpu.[Public] = 1
      left join AggregatedJson jpr
        on p.[Id] = jpr.[PlayerId] and jpr.[Public] = 0
      left join [Relay].[PLayerActiveView] pa
        on p.[Id] = pa.[PlayerId]
      left join InGamePlayers igp
        on p.[Id] = igp.[PlayerId]