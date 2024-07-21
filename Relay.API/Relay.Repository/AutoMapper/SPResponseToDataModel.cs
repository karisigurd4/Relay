using AutoMapper;
using Newtonsoft.Json;
using Relay.DataModel;

namespace Relay.Repository.AutoMapper
{
  public class SPResponseToDataModel : Profile
  {
    public SPResponseToDataModel()
    {
      CreateMap<GetPlayerSPResponseJson, GetPlayerSPResponse>()
        .ForMember(dst => dst.Player, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Player>(src.PlayerJson)))
        .ForPath(dst => dst.Player.PublicPlayerData, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<PlayerData[]>(src.PublicPlayerDataJson)))
        .ForPath(dst => dst.Player.PrivatePlayerData, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<PlayerData[]>(src.PrivatePlayerDataJson)));

      CreateMap<GetPlayerFriendsListSPResponseJson, GetPlayerFriendsListSPResponse>()
        .ForMember(dst => dst.Players, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Player[]>(src.PlayersJson)));

      CreateMap<GetPlayerPartySPResponseJson, GetPlayerPartySPResponse>()
        .ForMember(dst => dst.Party, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Party[]>(src.PartyJson)));

      CreateMap<GetNotificationMessagesSPResponseJson, GetNotificationMessagesSPResponse>()
        .ForMember(dst => dst.NotificationMessages, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<NotificationMessage[]>(src.NotificationMessagesJson)));

      CreateMap<SearchPlayersSPResponseJson, SearchPlayersSPResponse>()
              .ForMember(dst => dst.Players, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Player[]>((string)src.PlayersJson)));

      CreateMap<FindGameServerSPResponseJson, FindGameServerSPResponse>()
        .ForMember(dst => dst.GameServer, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<DataModel.GameServer>(src.GameServerJson)));

      CreateMap<GetGameServerByIdSPResponseJson, GetGameServerByIdSPResponse>()
        .ForMember(dst => dst.GameServer, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<DataModel.GameServer>(src.GameServerJson)));

      CreateMap<GetGameServerInfoByIdSPResponseJson, GetGameServerInfoByIdSPResponse>()
        .ForMember(dst => dst.Players, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<GameServerPlayerInfo[]>(src.PlayersJson)));

      CreateMap<GetGameServerHostsSPResponseJson, GetGameServerHostsSPResponse>()
          .ForMember(dst => dst.Hosts, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<GameServerHost[]>(src.HostsJson)));

      CreateMap<BrowseGameServersSPResponseJson, BrowseGameServersSPResponse>()
          .ForMember(dst => dst.GameServerList, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<GameServerListEntry[]>(src.GameServerListJson)));

      CreateMap<GetProjectSubscriptionSPResponseJson, GetProjectSubscriptionSPResponse>()
        .ForMember(dst => dst.ProjectSubsciption, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<ProjectSubsciption>(src.ProjectSubscriptionJson)));

      CreateMap<GetGameServerStatisticsSPResponseJson, GetGameServerStatisticsSPResponse>()
        .ForMember(dst => dst.GameServerStatistics, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<GameServerStatistics[]>(src.ResultsJson)));

      CreateMap<GetPlayerStatisticsSPResponseJson, GetPlayerStatisticsSPResponse>()
        .ForMember(dst => dst.PlayerStatistics, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<PlayerStatistics[]>(src.ResultsJson)));

      CreateMap<GetGameServerByCodeSPResponseJson, GetGameServerByCodeSPResponse>()
        .ForMember(dst => dst.GameServer, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<GameServerListEntry>(src.ResultJson)));
    }
  }
}
