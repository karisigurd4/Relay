namespace Relay.Implementation
{
  using Relay.Contracts;

  public static class FindGameServerResponseFactory
  {
    public static FindGameServerResponse Create(bool success, string message) => new FindGameServerResponse()
    {
      Success = false,
      Message = message
    };
  }
}
