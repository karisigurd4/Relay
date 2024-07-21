namespace Relay.Implementation.Implementation
{
  using global::AutoMapper;
  using Relay.Contracts;
  using Relay.Core.Factories;
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.Implementation.Interfaces;
  using Relay.Repository.Interfaces;
  using System;

  public class NotificationMessageManager : INotificationMessageManager
  {
    private readonly IMapper mapper;
    private readonly INotificationMessageRepository notificationMessageRepository;
    private readonly IPlayerRepository playerRepository;
    private readonly IPartyRepository partyRepository;

    public NotificationMessageManager(IMapper mapper, INotificationMessageRepository notificationMessageRepository, IPlayerRepository playerRepository, IPartyRepository partyRepository)
    {
      this.mapper = mapper;
      this.notificationMessageRepository = notificationMessageRepository;
      this.playerRepository = playerRepository;
      this.partyRepository = partyRepository;
    }

    public AnswerNotificationResponse AnswerNotification(AnswerNotificationRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var notification = notificationMessageRepository.GetNotificationMessageById(uw, new GetNotificationMessageByIdSPRequest()
        {
          Id = request.NotificationMessageId
        });

        var notificationReceiverPlayer = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId),
          ApiKey = request.PlayerApiKey
        });

        if (notificationReceiverPlayer.Player.Id != notification.ToPlayerId)
        {
          AnswerNotificationResponseFactory.Create(false, "Not your notification");
        }

        if (notification == null)
        {
          return AnswerNotificationResponseFactory.Create(false, $"Could not retrieve a notification with the specified Id.");
        }

        if (notification.Type == NotificationMessageType.Info)
        {
          return AnswerNotificationResponseFactory.Create(false, $"Cannot perform answer operation on 'Info' type notifications.");
        }

        var hideNotificationResponse = notificationMessageRepository.HideNotification(uw, new HideNotificationSPRequest()
        {
          Id = request.NotificationMessageId
        });

        if (!hideNotificationResponse.Success)
        {
          return AnswerNotificationResponseFactory.Create(false, $"An unexpected error occurred when handling the notification answer.");
        }

        if (request.Answer == Answer.Yes)
        {
          if (notification.Type == NotificationMessageType.FriendRequest)
          {
            return AddPlayerFriends(uw, notification.ReferenceId, notification.ToPlayerId, request.ProjectId);
          }
          else
          {
            return AddPlayerToParty(uw, notification.ReferenceId, notification.ToPlayerId, request.ProjectId);
          }
        }

        return AnswerNotificationResponseFactory.Create();
      }
    }

    private AnswerNotificationResponse AddPlayerToParty(IRelayUnitOfWork uw, int partyId, int toPlayerId, string projectId)
    {
      var addPlayerToPartyResponse = partyRepository.AddPartyPlayer(uw, new AddPartyPlayerSPRequest()
      {
        PartyId = partyId,
        PlayerId = toPlayerId
      });

      if (!addPlayerToPartyResponse.Success)
      {
        return AnswerNotificationResponseFactory.Create(false, $"An unexpected error occurred when adding player to the party");
      }

      notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
      {
        Data = $"You've joined the party!",
        ToPlayerId = toPlayerId,
        Type = NotificationMessageType.Info,
      });

      return AnswerNotificationResponseFactory.Create();
    }

    private AnswerNotificationResponse AddPlayerFriends(IRelayUnitOfWork uw, int player1Id, int player2Id, string projectId)
    {
      var player1 = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
      {
        ProjectId = Guid.Parse(projectId),
        Id = player1Id
      });

      if (!player1.Success)
      {
        return AnswerNotificationResponseFactory.Create(false, "The receiving player could not be retrieved");
      }

      var player2 = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
      {
        ProjectId = Guid.Parse(projectId),
        Id = player2Id,
      });

      if (!player2.Success)
      {
        return AnswerNotificationResponseFactory.Create(false, "The sending player could not be retrieved");
      }

      var addPlayerFriendResponse = playerRepository.AddPlayerFriend(uw, new AddPlayerFriendSPRequest()
      {
        Player1Id = player1Id,
        Player2Id = player2Id
      });

      if (!addPlayerFriendResponse.Success)
      {
        return AnswerNotificationResponseFactory.Create(false, "An unexpected error occurred when adding player friend");
      }

      notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
      {
        Data = $"You and {player2.Player.Name} are now friends!",
        ToPlayerId = player1.Player.Id,
        Type = NotificationMessageType.Info,
      });

      notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
      {
        Data = $"You and {player1.Player.Name} are now friends!",
        ToPlayerId = player2.Player.Id,
        Type = NotificationMessageType.Info,
      });

      return AnswerNotificationResponseFactory.Create();
    }

    public GetNotificationMessagesResponse GetNotificationMessages(GetNotificationMessagesRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetNotificationMessagesResponse>(notificationMessageRepository.GetNotificationMessages(uw, mapper.Map<GetNotificationMessagesSPRequest>(request)));
      }
    }

    public GetUnreadNotificationMessagesCountResponse GetUnreadNotificationMessagesCount(GetUnreadNotificationMessagesCountRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetUnreadNotificationMessagesCountResponse>(notificationMessageRepository.GetUnreadNotificationMessagesCount(uw, mapper.Map<GetUnreadNotificationMessagesCountSPRequest>(request)));
      }
    }
  }
}
