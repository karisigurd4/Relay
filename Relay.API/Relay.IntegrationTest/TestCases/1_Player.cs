using Relay.Contracts;
using Relay.IntegrationTest.ApiClients;
using Relay.IntegrationTest.Attributes;
using Relay.IntegrationTest.Interfaces;
using Relay.IntegrationTest.Utils;
using System;

namespace Relay.IntegrationTest.TestCases
{
  [IntegrationTest(0, "Tests the player registration process")]
  public class _1_Player : IIntegrationTest
  {
    public static RegisterPlayerResponse RegisterPlayerResponse;
    public static string ProjectId = Guid.NewGuid().ToString();

    public void AfterExecution()
    {
    }

    public void BeforeExecution()
    {
    }

    [IntegrationTestCase(0, "Tests the player registration process")]
    public bool RegisterPlayer()
    {
      RegisterPlayerResponse = PlayerClient.RegisterPlayer(new RegisterPlayerRequest()
      {
        Name = $"IntegrationTest_Player_{new Random((int)DateTime.Now.Ticks).Next().ToString()}",
        ProjectId = ProjectId
      });

      if (!RegisterPlayerResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (string.IsNullOrWhiteSpace(RegisterPlayerResponse.ApiKey))
      {
        IntegrationTestLogger.Error($"Did not receive a valid api key for player registration");
        return false;
      }

      return true;
    }

    [IntegrationTestCase(1, "Tests the private data association")]
    public bool SetPlayerPrivateData()
    {
      var response = PlayerClient.UpdatePlayer(new UpdatePlayerRequest()
      {
        ApiKey = RegisterPlayerResponse.ApiKey,
        Key = "PrivateKey",
        Value = "PrivateValue",
        Public = false
      });

      if (!response.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      IntegrationTestLogger.Info($"Retrieving player by api key and validating private data");
      var getPlayerResponse = PlayerClient.GetPlayer(new GetPlayerRequest()
      {
        ProjectId = ProjectId,
        ApiKey = RegisterPlayerResponse.ApiKey
      });

      if (!getPlayerResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (!getPlayerResponse.Player.Data_Private.ContainsKey("PrivateKey"))
      {
        IntegrationTestLogger.Error($"Player response does not contain private key");
        return false;
      }
      else if (getPlayerResponse.Player.Data_Private["PrivateKey"] != "PrivateValue")
      {
        IntegrationTestLogger.Error($"Player contains private key, but value is not PrivateValue, is {getPlayerResponse.Player.Data_Private["PrivateKey"]}");
        return false;
      }

      IntegrationTestLogger.Info($"Retrieving player by id and validating private data");
      var getPlayerByIdResponse = PlayerClient.GetPlayer(new GetPlayerRequest()
      {
        ProjectId = ProjectId,
        Id = RegisterPlayerResponse.Id
      });

      if (!getPlayerByIdResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (getPlayerByIdResponse.Player.Data_Private.ContainsKey("PrivateKey"))
      {
        IntegrationTestLogger.Error($"Player response contains private dictionary, should be null when getting players by id only");
        return false;
      }

      return true;
    }

    [IntegrationTestCase(1, "Tests the public data association")]
    public bool SetPlayerPublicData()
    {
      var response = PlayerClient.UpdatePlayer(new UpdatePlayerRequest()
      {
        ApiKey = RegisterPlayerResponse.ApiKey,
        Key = "PublicKey",
        Value = "PublicValue",
        Public = true
      });

      if (!response.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }

      IntegrationTestLogger.Info($"Retrieving player by api key and validating public data");
      var getPlayerResponse = PlayerClient.GetPlayer(new GetPlayerRequest()
      {
        ProjectId = ProjectId,
        Id = RegisterPlayerResponse.Id
      });

      if (!getPlayerResponse.Success)
      {
        IntegrationTestLogger.Error($"Success field is false");
        return false;
      }
      else if (!getPlayerResponse.Player.Data_Public.ContainsKey("PublicKey"))
      {
        IntegrationTestLogger.Error($"Player response does not contain private key");
        return false;
      }
      else if (getPlayerResponse.Player.Data_Public["PublicKey"] != "PublicValue")
      {
        IntegrationTestLogger.Error($"Player contains public key, but value is not PublicValue, is {getPlayerResponse.Player.Data_Private["PublicKey"]}");
        return false;
      }

      return true;
    }
  }
}
