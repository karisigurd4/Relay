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

  public class PartyManager : IPartyManager
  {
    private IMapper mapper;
    private IPartyRepository partyRepository;
    private INotificationMessageRepository notificationMessageRepository;
    private IPlayerRepository playerRepository;

    public PartyManager(IPartyRepository partyRepository, INotificationMessageRepository notificationMessageRepository, IPlayerRepository playerRepository, IMapper mapper)
    {
      this.mapper = mapper;
      this.partyRepository = partyRepository;
      this.notificationMessageRepository = notificationMessageRepository;
      this.playerRepository = playerRepository;
    }

    public GetPlayerPartyResponse GetPlayerParty(GetPlayerPartyRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<GetPlayerPartyResponse>(partyRepository.GetPlayerParty(uw, mapper.Map<GetPlayerPartySPRequest>(request)));
      }
    }

    public InvitePlayerToPartyResponse InvitePlayerToParty(InvitePlayerToPartyRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        var sendingPlayer = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId),
          ApiKey = request.FromPlayerApiKey
        });

        var sendingPlayerParty = partyRepository.GetPlayerParty(uw, new GetPlayerPartySPRequest()
        {
          PlayerId = sendingPlayer.Player.Id
        });

        if (sendingPlayerParty.Party == null || sendingPlayerParty.Party.Length == 0)
        {
          var createPartyResponse = partyRepository.CreateParty(uw, new CreatePartySPRequest()
          {
            PartyLeaderPlayerId = sendingPlayer.Player.Id
          });
        }

        var party = partyRepository.GetPlayerParty(uw, new GetPlayerPartySPRequest()
        {
          PlayerId = sendingPlayer.Player.Id
        }).Party.First();

        var receivingPlayer = playerRepository.GetPlayer(uw, new GetPlayerSPRequest()
        {
          ProjectId = Guid.Parse(request.ProjectId),
          Id = request.ToPlayerId
        });

        if (!sendingPlayer.Success || !receivingPlayer.Success)
        {
          return new InvitePlayerToPartyResponse()
          {
            Success = false,
            OperationResult = OperationResult.Fault_BadRequestParameters
          };
        }

        var receivingPlayerParty = partyRepository.GetPlayerParty(uw, new GetPlayerPartySPRequest()
        {
          PlayerId = receivingPlayer.Player.Id
        });

        if (receivingPlayerParty.Party != null && receivingPlayerParty.Party.Length != 0)
        {
          return new InvitePlayerToPartyResponse()
          {
            Message = receivingPlayer.Player.Name + " is already in a party.",
            Success = false,
            OperationResult = OperationResult.Success
          };
        }

        notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
        {
          Data = $"You've sent a party invite to {receivingPlayer.Player.Name}",
          Type = NotificationMessageType.Info,
          ToPlayerId = sendingPlayer.Player.Id
        });

        notificationMessageRepository.AddNotificationMessage(uw, new AddNotificationMessageSPRequest()
        {
          Data = $"You've received a party invitation from {sendingPlayer.Player.Name}! Click here to respond.",
          ReferenceId = party.PartyId,
          Type = NotificationMessageType.PartyInvite,
          ToPlayerId = receivingPlayer.Player.Id
        });

        return new InvitePlayerToPartyResponse()
        {
          Success = true
        };
      }
    }

    public KickPartyPlayerResponse KickPartyPlayer(KickPartyPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<KickPartyPlayerResponse>(partyRepository.KickPartyPlayer(uw, mapper.Map<KickPartyPlayerSPRequest>(request)));
      }
    }

    public SetPartyLeaderPlayerResponse SetPartyLeaderPlayer(SetPartyLeaderPlayerRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<SetPartyLeaderPlayerResponse>(partyRepository.SetPartyLeaderPlayer(uw, mapper.Map<SetPartyLeaderPlayerSPRequest>(request)));
      }
    }

    public LeavePartyResponse LeaveParty(LeavePartyRequest request)
    {
      using (var uw = RelayUnitOfWorkFactory.Create())
      {
        return mapper.Map<LeavePartyResponse>(partyRepository.LeaveParty(uw, mapper.Map<LeavePartySPRequest>(request)));
      }
    }
  }
}
