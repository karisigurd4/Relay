namespace Relay.GameServer.Core.Factories
{
  using Relay.GameServer.Core.Implementation.Relay.Core.Implementation;

  public static class RelayUnitOfWorkFactory
  {
    public static RelayUnitOfWork Create()
    {
      return new RelayUnitOfWork();
    }
  }
}
