namespace Relay.GameServer.Core.Interfaces
{
  using System;
  using System.Data;

  public interface IRelayUnitOfWork : IDisposable
  {
    Response Execute<Response>(Func<IDbConnection, Response> executeFunc);
    Response ExecuteSP<Response>(string spName, object parameters);
  }
}
