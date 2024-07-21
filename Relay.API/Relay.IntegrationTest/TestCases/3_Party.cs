namespace Relay.IntegrationTest.TestCases
{
  using Relay.IntegrationTest.ApiClients;
  using Relay.IntegrationTest.Attributes;
  using Relay.IntegrationTest.Interfaces;
  using Relay.IntegrationTest.Utils;
  using System.Linq;

  [IntegrationTest(2, "Tests the player party process")]
  public class _3_Party : IIntegrationTest
  {
    public void AfterExecution()
    {
    }

    public void BeforeExecution()
    {
    }

    [IntegrationTestCase(0, "Send a party invite from player 1 to player 2")]
    public bool SendPartyInvite()
    {
      var inviteToPartyResponse = PartyClient.InvitePlayerToParty(new Contracts.InvitePlayerToPartyRequest()
      {
        ProjectId = _1_Player.ProjectId,
        FromPlayerApiKey = _1_Player.RegisterPlayerResponse.ApiKey,
        ToPlayerId = _2_PlayerFriend.RegisterPlayerFriendResponse.Id
      });

      if (!inviteToPartyResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      var player2Notification = NotificationMessagesClient.GetNotificationMessages(new Contracts.GetNotificationMessagesRequest()
      {
        ApiKey = _2_PlayerFriend.RegisterPlayerFriendResponse.ApiKey,
        Count = 10,
        Offset = 0
      }).NotificationMessages.Last();

      if (player2Notification.Type != "PartyInvite")
      {
        IntegrationTestLogger.Error($"Notification for player 2 is not PartyInvite, is {player2Notification.Type}");
        return false;
      }
      return true;
    }

    [IntegrationTestCase(0, "Player 2 accepts party invite")]
    public bool AcceptPartyInvite()
    {
      var player2Notification = NotificationMessagesClient.GetNotificationMessages(new Contracts.GetNotificationMessagesRequest()
      {
        ApiKey = _2_PlayerFriend.RegisterPlayerFriendResponse.ApiKey,
        Count = 10,
        Offset = 0
      }).NotificationMessages.Last();

      var answerResponse = NotificationMessagesClient.AnswerNotificationMessage(new Contracts.AnswerNotificationRequest()
      {
        ProjectId = _1_Player.ProjectId,
        Answer = Contracts.Answer.Yes,
        NotificationMessageId = player2Notification.Id,
        PlayerApiKey = _2_PlayerFriend.RegisterPlayerFriendResponse.ApiKey
      });

      if (!answerResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      var player1Party = PartyClient.GetPlayerParty(new Contracts.GetPlayerPartyRequest()
      {
        PlayerId = _1_Player.RegisterPlayerResponse.Id
      });

      if (!player1Party.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player1Party.Party.Length != 2)
      {
        IntegrationTestLogger.Error($"Party should contain two players, is {player1Party.Party.Length}");
        return false;
      }

      var player2Party = PartyClient.GetPlayerParty(new Contracts.GetPlayerPartyRequest()
      {
        PlayerId = _2_PlayerFriend.RegisterPlayerFriendResponse.Id
      });

      if (!player2Party.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player2Party.Party.Length != 2)
      {
        IntegrationTestLogger.Error($"Party should contain two players, is {player2Party.Party.Length}");
        return false;
      }

      return true;
    }
  }
}
