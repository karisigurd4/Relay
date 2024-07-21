namespace Relay.Implementation.AutoMapper
{
  using global::AutoMapper;
  using Relay.Contracts;
  using Relay.DataModel;
  using System;
  using System.Linq;

  public class ContractRequestToDataModelRequest : Profile
  {
    public ContractRequestToDataModelRequest()
    {
      CreateMap<DataModel.Player, Contracts.Player>()
        .ForMember(src => src.Data_Private, opt => opt.MapFrom(dst => dst.PrivatePlayerData.ToDictionary(x => x.Key, x => x.Value)))
        .ForMember(src => src.Data_Public, opt => opt.MapFrom(dst => dst.PublicPlayerData.ToDictionary(x => x.Key, x => x.Value)));

      CreateMap<DataModel.Party, Contracts.Party>();

      CreateMap<DataModel.NotificationMessage, Contracts.NotificationMessage>();

      CreateMap<GetNotificationMessagesRequest, GetNotificationMessagesSPRequest>();
      CreateMap<GetNotificationMessagesSPResponse, GetNotificationMessagesResponse>();

      CreateMap<GetUnreadNotificationMessagesCountRequest, GetUnreadNotificationMessagesCountSPRequest>();
      CreateMap<GetUnreadNotificationMessagesCountSPResponse, GetUnreadNotificationMessagesCountResponse>();

      CreateMap<AddGameServerPlayerRequest, AddGameServerPlayerSPRequest>();
      CreateMap<AddGameServerPlayerSPResponse, AddGameServerPlayerResponse>();

      CreateMap<KickPartyPlayerRequest, KickPartyPlayerSPRequest>();
      CreateMap<KickPartyPlayerSPResponse, KickPartyPlayerResponse>();

      CreateMap<SetPartyLeaderPlayerRequest, SetPartyLeaderPlayerSPRequest>();
      CreateMap<SetPartyLeaderPlayerSPResponse, SetPartyLeaderPlayerResponse>();

      CreateMap<GetPlayerFriendsListRequest, GetPlayerFriendsListSPRequest>();
      CreateMap<GetPlayerFriendsListSPResponse, GetPlayerFriendsListResponse>();

      CreateMap<GetPlayerRequest, GetPlayerSPRequest>()
        .ForMember(dst => dst.ProjectId, opt => opt.MapFrom(src => Guid.Parse(src.ProjectId)));

      CreateMap<GetPlayerSPResponse, GetPlayerResponse>();

      CreateMap<RegisterPlayerRequest, RegisterPlayerSPRequest>();
      CreateMap<RegisterPlayerSPResponse, RegisterPlayerResponse>();

      CreateMap<RemovePlayerFromFriendListRequest, RemovePlayerFriendSPRequest>();
      CreateMap<RemovePlayerFriendSPResponse, RemovePlayerFromFriendListResponse>();

      CreateMap<SearchPlayersRequest, SearchPlayersSPRequest>()
        .ForMember(dst => dst.ProjectId, opt => opt.MapFrom(src => Guid.Parse(src.ProjectId)));

      CreateMap<SearchPlayersSPResponse, SearchPlayersResponse>();

      CreateMap<UpdatePlayerRequest, UpdatePlayerSPRequest>();
      CreateMap<UpdatePlayerSPResponse, UpdatePlayerResponse>();

      CreateMap<RegisterPlayerRequest, RegisterPlayerSPRequest>();
      CreateMap<RegisterPlayerSPResponse, RegisterPlayerResponse>();

      CreateMap<AnswerNotificationRequest, GetNotificationMessageByIdSPRequest>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.NotificationMessageId));

      CreateMap<GetPlayerPartyRequest, GetPlayerPartySPRequest>();
      CreateMap<GetPlayerPartySPResponse, GetPlayerPartyResponse>();

      CreateMap<DataModel.GameServer, Contracts.GameServer>();

      CreateMap<FindGameServerRequest, FindGameServerSPRequest>();
      CreateMap<FindGameServerSPResponse, FindGameServerResponse>();

      CreateMap<DataModel.GameServerPlayerInfo, Contracts.GameServerPlayerInfo>();

      CreateMap<GetGameServerInfoByIdRequest, GetGameServerInfoByIdSPRequest>();
      CreateMap<GetGameServerInfoByIdSPResponse, GetGameServerInfoByIdResponse>();

      CreateMap<IncrementPlayerScoreRequest, IncrementPlayerScoreSPRequest>();
      CreateMap<IncrementPlayerScoreSPResponse, IncrementPlayerScoreResponse>();

      CreateMap<SetPlayerNameRequest, SetPlayerNameSPRequest>();
      CreateMap<SetPlayerNameSPResponse, SetPlayerNameResponse>();

      CreateMap<LeavePartyRequest, LeavePartySPRequest>();
      CreateMap<LeavePartySPResponse, LeavePartyResponse>();

      CreateMap<DataModel.GameServerHost, Contracts.GameServerHost>();
      CreateMap<DataModel.GameServerHostStatus, Contracts.GameServerHostStatus>();

      CreateMap<GetGameServerHostsRequest, GetGameServerHostsSPRequest>();
      CreateMap<GetGameServerHostsSPResponse, GetGameServerHostsResponse>();

      CreateMap<DataModel.GameServerListEntry, Contracts.GameServerListEntry>();

      CreateMap<BrowseGameServersRequest, BrowseGameServersSPRequest>();
      CreateMap<BrowseGameServersSPResponse, BrowseGameServersResponse>();

      CreateMap<Contracts.Period, DataModel.Period>();

      CreateMap<DataModel.GameServerStatistics, Contracts.GameServerStatistics>();

      CreateMap<GetGameServerStatisticsRequest, GetGameServerStatisticsSPRequest>();
      CreateMap<GetGameServerStatisticsSPResponse, GetGameServerStatisticsResponse>();

      CreateMap<DataModel.PlayerStatistics, Contracts.PlayerStatistics>();

      CreateMap<GetPlayerStatisticsRequest, GetPlayerStatisticsSPRequest>();
      CreateMap<GetPlayerStatisticsSPResponse, GetPlayerStatisticsResponse>();

      CreateMap<GetGameServerByCodeRequest, GetGameServerByCodeSPRequest>();
      CreateMap<GetGameServerByCodeSPResponse, GetGameServerByCodeResponse>();
    }
  }
}
