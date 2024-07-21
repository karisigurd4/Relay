create table [Relay].[ProjectSubscription]
(
  [ProjectId] uniqueidentifier,
  [SubscriptionId] nvarchar(256),
  [Active] bit default 1,
  [CheckExpirationFlag] bit default 1,
  [LastCheckedStatusDate] datetime2 default getutcdate(),

  constraint PK_Relay_ProjectSubscription_Id primary key ([ProjectId])
)