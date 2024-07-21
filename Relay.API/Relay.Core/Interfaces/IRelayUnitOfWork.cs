using System;
using System.Data;

namespace Relay.Core.Interfaces
{
  public interface IRelayUnitOfWork : IDisposable
  {
    Response Execute<Response>(Func<IDbConnection, Response> executeFunc);
    Response ExecuteSP<Response>(string spName, object parameters);
  }
}
