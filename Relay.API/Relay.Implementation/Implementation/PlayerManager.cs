namespace Relay.Implementation.Implementation
{
  using global::AutoMapper;
  using Relay.Contracts;
  using Relay.Core.Factories;
  using Relay.DataModel;
  using Relay.Implementation.Interfaces;
  using Relay.Repository.Interfaces;
  using System;
  using System.Linq;

  public class PlayerManager : IPlayerManager
  {
    private readonly IMapper mapper;
    private readonly IPlayerRepository playerRepository;
    private readonly INotificationMessageRepository notificationMessageRepository;

    public PlayerManager(IMapper mapper, IPlayerRepository playerRepository, INotificationMessageRepository notificationMessageRepository)
    {
      this.mapper = mapper;
      this.playerRepository = playerRepository;
      this.notificationMessageRepository = notificationMessageRepository;
    }

    public GetPlayerFriendsListResponse GetPlayerFriendsList(GetPlayerFriendsListRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetPlayerFriendsListResponse>(playerRepository.GetPlayerFriendsList(uw, mapper.Map<GetPlayerFriendsListSPRequest>(request)));
      }
    }

    public GetPlayerResponse GetPlayer(GetPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetPlayerResponse>(playerRepository.GetPlayer(uw, mapper.Map<GetPlayerSPRequest>(request)));
      }
    }

    public RegisterPlayerResponse RegisterPlayer(RegisterPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var registerPlayerResponse = mapper.Map<RegisterPlayerResponse>(playerRepository.RegisterPlayer(uw, mapper.Map<RegisterPlayerSPRequest>(request)));
        if (!registerPlayerResponse.Success)
        {
          return new RegisterPlayerResponse()
          {
            Success = false,
            Message = "An unexpected error occurred when registering player"
          };
        }

        notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
        {
          Data = $"Welcome to the game {request.Name}!",
          ToPlayerId = registerPlayerResponse.Id,
          Type = NotificationMessageType.Info
        });

        return registerPlayerResponse;
      }
    }

    public RemovePlayerFromFriendListResponse RemovePlayerFromFriendsList(RemovePlayerFromFriendListRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<RemovePlayerFromFriendListResponse>(playerRepository.RemovePlayerFriend(uw, mapper.Map<RemovePlayerFriendSPRequest>(request)));
      }
    }

    public SearchPlayersResponse SearchPlayers(SearchPlayersRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<SearchPlayersResponse>(playerRepository.SearchPlayers(uw, mapper.Map<SearchPlayersSPRequest>(request)));
      }
    }

    public SendPlayerFriendRequestResponse SendPlayerFriendRequest(SendPlayerFriendRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var sendingPlayer = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId),
          ApiKey = request.FromPlayerApiKey
        });

        var sendingPlayerFriends = playerRepository.GetPlayerFriendsList(uw, new GetPlayerFriendsListSPRequest()
        {
          ApiKey = Guid.Parse(request.FromPlayerApiKey)
        });

        var receivingPlayer = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId),
          Id = request.ToPlayerId
        });

        if (sendingPlayerFriends.Players.Any(x => x.Id == request.ToPlayerId))
        {
          return new SendPlayerFriendRequestResponse()
          {
            Message = "You are already friends with " + receivingPlayer.Player.Name,
            Success = false,
            OperationResult = OperationResult.Success
          };
        }

        if (!sendingPlayer.Success || !receivingPlayer.Success)
        {
          return new SendPlayerFriendRequestResponse()
          {
            Success = false,
            OperationResult = OperationResult.Fault_BadRequestParameters
          };
        }

        notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
        {
          Data = $"You've sent a friend request to {receivingPlayer.Player.Name}",
          Type = NotificationMessageType.Info,
          ToPlayerId = sendingPlayer.Player.Id
        });

        notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
        {
          Data = $"You've received a friend request from {sendingPlayer.Player.Name}! Click here to respond.",
          ReferenceId = sendingPlayer.Player.Id,
          Type = NotificationMessageType.FriendRequest,
          ToPlayerId = receivingPlayer.Player.Id
        });

        return new SendPlayerFriendRequestResponse()
        {
          Success = true
        };
      }
    }

    public UpdatePlayerResponse UpdatePlayer(UpdatePlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<UpdatePlayerResponse>(playerRepository.UpdatePlayer(uw, mapper.Map<UpdatePlayerSPRequest>(request)));
      }
    }

    public IncrementPlayerScoreResponse IncrementPlayerScore(IncrementPlayerScoreRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<IncrementPlayerScoreResponse>(playerRepository.IncrementPlayerScore(uw, mapper.Map<IncrementPlayerScoreSPRequest>(request)));
      }
    }

    public SetPlayerNameResponse SetPlayerName(SetPlayerNameRequest request)
    {
      // Unity text inputs add zero-width whitespace UTC characters to strings. This is just baffling.
      request.NewPlayerName = request.NewPlayerName.Replace("\u200B", "");

      if (string.IsNullOrWhiteSpace(request.NewPlayerName))
      {
        return new SetPlayerNameResponse()
        {
          Success = false,
          Message = "New name cannot be empty"
        };
      }

      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<SetPlayerNameResponse>(playerRepository.SetPlayerName(uw, mapper.Map<SetPlayerNameSPRequest>(request)));
      }
    }

    public GetPlayerStatisticsResponse GetPlayerStatistics(GetPlayerStatisticsRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetPlayerStatisticsResponse>(playerRepository.GetPlayerStatistics(uw, mapper.Map<GetPlayerStatisticsSPRequest>(request)));
      }
    }
  }
}
