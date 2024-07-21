create table [Relay].[GameServerHost]
(
  [HostName] nvarchar(256) default '',
  [CpuUsage] float default 0,
  [MemoryUsage] float default 0,
  [LastUpdate] datetime2 default getutcdate(),

  constraint PK_Relay_GameServerHost primary key ([HostName])
)