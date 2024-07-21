namespace Relay.IntegrationTest.ApiClients
{
  using Relay.Contracts;
  using Relay.IntegrationTest.Utils;
  using System.Net.Http;

  public static class NotificationMessagesClient
  {
    private static HttpClient httpClient = new HttpClient();

    public static GetUnreadNotificationMessagesCountResponse GetUnreadNotificationMessagesCount(GetUnreadNotificationMessagesCountRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.NotificationMessageRoute}/count");
      var response = HttpClientUtils.Post(httpClient, Constants.NotificationMessageRoute + "/count", request);
      return HttpClientUtils.GetContent<GetUnreadNotificationMessagesCountResponse>(response);
    }

    public static GetNotificationMessagesResponse GetNotificationMessages(GetNotificationMessagesRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.NotificationMessageRoute}/");
      var response = HttpClientUtils.Post(httpClient, Constants.NotificationMessageRoute + "/", request);
      return HttpClientUtils.GetContent<GetNotificationMessagesResponse>(response);
    }

    public static AnswerNotificationResponse AnswerNotificationMessage(AnswerNotificationRequest request)
    {
      IntegrationTestLogger.Info($"Calling {Constants.ApiRoute + Constants.NotificationMessageRoute}/answer");
      var response = HttpClientUtils.Post(httpClient, Constants.NotificationMessageRoute + "/answer", request);
      return HttpClientUtils.GetContent<AnswerNotificationResponse>(response);
    }
  }
}
