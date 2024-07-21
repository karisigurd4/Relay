using Relay.Core.Implementation;

namespace Relay.Core.Factories
{
  public static class RelayUnitOfWorkFactory
  {
    public static RelayUnitOfWork Create()
    {
      return new RelayUnitOfWork();
    }
  }
}
