namespace Relay.IntegrationTest.TestCases
{
  using Relay.Contracts;
  using Relay.IntegrationTest.ApiClients;
  using Relay.IntegrationTest.Attributes;
  using Relay.IntegrationTest.Interfaces;
  using Relay.IntegrationTest.Utils;
  using System;
  using System.Linq;

  [IntegrationTest(1, "Tests the player registration process")]
  public class _2_PlayerFriend : IIntegrationTest
  {
    public static RegisterPlayerResponse RegisterPlayerFriendResponse;

    public void AfterExecution()
    {
    }

    public void BeforeExecution()
    {
    }

    [IntegrationTestCase(0, "Register a player to send a friend request to")]
    public bool RegisterPlayerFriend()
    {
      RegisterPlayerFriendResponse = PlayerClient.RegisterPlayer(new RegisterPlayerRequest()
      {
        ProjectId = _1_Player.ProjectId,
        Name = $"IntegrationTest_Player_{new Random((int)DateTime.Now.Ticks).Next().ToString()}"
      });

      if (!RegisterPlayerFriendResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (string.IsNullOrWhiteSpace(RegisterPlayerFriendResponse.ApiKey))
      {
        IntegrationTestLogger.Error($"Did not receive a valid api key for player registration");
        return false;
      }
      return true;
    }

    [IntegrationTestCase(1, "Send a friend request from Test_1 player to the new friend player")]
    public bool SendFriendRequest()
    {
      var sendFriendRequestResponse = PlayerClient.SendPlayerFriendRequest(new SendPlayerFriendRequest()
      {
        ProjectId = _1_Player.ProjectId,
        FromPlayerApiKey = _1_Player.RegisterPlayerResponse.ApiKey,
        ToPlayerId = RegisterPlayerFriendResponse.Id
      });

      if (!sendFriendRequestResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      var player1Notifications = NotificationMessagesClient.GetNotificationMessages(new GetNotificationMessagesRequest()
      {
        ApiKey = _1_Player.RegisterPlayerResponse.ApiKey,
        Count = 10,
        Offset = 0
      });

      if (!player1Notifications.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player1Notifications.NotificationMessages == null || player1Notifications.NotificationMessages.Length == 0)
      {
        IntegrationTestLogger.Error($"Notifications is null or empty");
        return false;
      }
      else if (player1Notifications.NotificationMessages.Last().Type != "Info")
      {
        IntegrationTestLogger.Error($"The Last notification for player 1 should be info, is {player1Notifications.NotificationMessages.Last().Type}");
        return false;
      }

      var player2Notifications = NotificationMessagesClient.GetNotificationMessages(new GetNotificationMessagesRequest()
      {
        ApiKey = RegisterPlayerFriendResponse.ApiKey,
        Count = 10,
        Offset = 0
      });

      if (!player2Notifications.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player2Notifications.NotificationMessages == null || player1Notifications.NotificationMessages.Length == 0)
      {
        IntegrationTestLogger.Error($"Notifications is null or empty");
        return false;
      }
      else if (player2Notifications.NotificationMessages.Last().Type != "FriendRequest")
      {
        IntegrationTestLogger.Error($"The Last notification for player 2 should be FriendRequest is {player2Notifications.NotificationMessages.Last().Type}");
        return false;
      }

      return true;
    }

    [IntegrationTestCase(1, "Player 2 accepts the friend request from player 1")]
    public bool AcceptFriendRequest()
    {
      var player2NotificationFriendRequest = NotificationMessagesClient.GetNotificationMessages(new GetNotificationMessagesRequest()
      {
        ApiKey = RegisterPlayerFriendResponse.ApiKey,
        Count = 10,
        Offset = 0
      }).NotificationMessages.Last();

      var answerNotificationRespone = NotificationMessagesClient.AnswerNotificationMessage(new AnswerNotificationRequest()
      {
        ProjectId = _1_Player.ProjectId,
        Answer = Answer.Yes,
        NotificationMessageId = player2NotificationFriendRequest.Id,
        PlayerApiKey = RegisterPlayerFriendResponse.ApiKey
      });

      if (!answerNotificationRespone.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      var player2Friends = PlayerClient.GetPlayerFriendsList(new GetPlayerFriendsListRequest()
      {
        ApiKey = RegisterPlayerFriendResponse.ApiKey
      });

      if (!player2Friends.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player2Friends.Players.Length == 0)
      {
        IntegrationTestLogger.Error($"Player 2 friend count is 0");
        return false;
      }

      var player1Friends = PlayerClient.GetPlayerFriendsList(new GetPlayerFriendsListRequest()
      {
        ApiKey = _1_Player.RegisterPlayerResponse.ApiKey
      });

      if (!player1Friends.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (player1Friends.Players.Length == 0)
      {
        IntegrationTestLogger.Error($"Player 1 friend count is 0");
        return false;
      }

      return true;
    }
  }
}
