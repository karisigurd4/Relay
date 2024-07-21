using Relay.GameServer.Contracts;
using Relay.GameServer.Core.Types;
using Relay.GameServer.Implementation.Interfaces;
using Riptide;
using Riptide.Utils;
using System.Collections.Generic;

namespace Relay.GameServer.Implementation.Implementation
{
  public class ClientConnectionManager : IClientConnectionManager
  {
    private List<ushort> connectedClientIds = new List<ushort>();
    private List<ushort> disconnectedClientIds = new List<ushort>();

    public ushort[] GetConnectedClientIds()
    {
      lock (connectedClientIds)
      {
        return connectedClientIds.ToArray();
      }
    }

    public void OnClientConnected(ServerConnectedEventArgs eventArgs, GameServerState gameServerState, int gameServerId, Server server)
    {
      try
      {
        RiptideLogger.Log(LogType.Info, $"Client connected");
        SendClientConnectionDetails(eventArgs.Client.Id, gameServerState, gameServerId, server);
        lock (connectedClientIds)
        {
          connectedClientIds.Add(eventArgs.Client.Id);
        }
      }
      catch (System.Exception e)
      {
        RiptideLogger.Log(LogType.Error, $"An exception occurred in OnClientConnected: {e.Message}");
      }
    }

    void SendClientConnectionDetails(ushort clientId, GameServerState gameServerState, int gameServerId, Server server)
    {
      RiptideLogger.Log(LogType.Info, $"Sending client {clientId} connection details");
      var response = Message.Create(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientConnectionDetails);
      response.AddUShort(clientId);
      response.AddInt(gameServerId);
      response.AddInt((int)gameServerState);
      server.Send(response, clientId);

      for (int i = 0; i < disconnectedClientIds.Count; i++)
      {
        var disconnectDetailsBroadcast = Message.Create(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientDisconnected);
        disconnectDetailsBroadcast.AddUShort(disconnectedClientIds[i]);
        server.Send(disconnectDetailsBroadcast, clientId);
      }
    }

    public void OnClientDisconnected(ushort clientId, int gameId, Server server)
    {
      RiptideLogger.Log(LogType.Info, $"Client with id: {clientId} disconnected");
      var disconnectDetailsBroadcast = Message.Create(MessageSendMode.Reliable, (ushort)GameServerMessageType.ClientDisconnected);
      disconnectDetailsBroadcast.AddUShort(clientId);
      server.SendToAll(disconnectDetailsBroadcast);
      if (connectedClientIds.Contains(clientId))
      {
        connectedClientIds.Remove(clientId);
      }
      disconnectedClientIds.Add(clientId);
    }
  }
}
