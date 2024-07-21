namespace Relay.GameServer.Core.Implementation
{
  using Dapper;
  using global::Relay.GameServer.Core.Utilities;
  using Interfaces;
  using System;
  using System.Data;
  using System.Data.SqlClient;

  namespace Relay.Core.Implementation
  {
    public class RelayUnitOfWork : IRelayUnitOfWork
    {
      private IDbConnection dbConnection = null;

      private void OpenConnection()
      {
        dbConnection = new SqlConnection(ConfigurationReader.GetGameServerConfiguration().DbConnectionString);
        dbConnection.Open();
      }

      private void CloseConnection()
      {
        if (dbConnection != null && dbConnection.State == ConnectionState.Open)
        {
          dbConnection.Close();
        }
      }

      public void Dispose()
      {
        CloseConnection();
      }

      public Response Execute<Response>(Func<IDbConnection, Response> executeFunc)
      {
        if (dbConnection == null || dbConnection.State != ConnectionState.Open)
        {
          OpenConnection();
        }

        return executeFunc(dbConnection);
      }

      public Response ExecuteSP<Response>(string spName, object parameters)
      {
        if (dbConnection == null || dbConnection.State != ConnectionState.Open)
        {
          OpenConnection();
        }

        return dbConnection.QueryFirstOrDefault<Response>(
          spName,
          parameters,
          commandType: CommandType.StoredProcedure
        );
      }
    }
  }

}
