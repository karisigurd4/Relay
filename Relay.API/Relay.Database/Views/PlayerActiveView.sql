create view [Relay].[PlayerActiveView]
as
  select 
      p.[Id] as [PlayerId],
      iif(count(pa.[Id]) > 0, 1, 0) as [PlayerActive]
    from [Relay].[Player] as p
      left join [Relay].[PlayerActivity] as pa
        on p.[Id] = pa.[PlayerId]
      where 
        datediff(minute, pa.[ActivityDateTime], getutcdate()) < 5
      group by p.[Id] 