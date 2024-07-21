using Relay.GameServer.Core;
using Riptide;
using System.Collections.Generic;

public class GameObjectState
{
  public int ApiPlayerId;
  public ushort ClientId;
  public int RelayInstanceId;
  public int NetworkInstanceId;

  public RelayGameObjectFieldState[] State;

  public Message AppendToMessage(Message m)
  {
    m.AddInt(ApiPlayerId);
    m.AddUShort(ClientId);
    m.AddInt(RelayInstanceId);
    m.AddInt(NetworkInstanceId);

    if (State == null || State.Length == 0)
    {
      m.AddInt(0);
    }
    else
    {
      m.AddInt(State.Length);

      for (int i = 0; i < State.Length; i++)
      {
        m.AddInt(State[i].FieldHashCode);
        m.AddInt(State[i].ComponentId);
        MessageSerializer.WriteObjectToMessage(m, State[i].Value);
      }
    }

    return m;
  }

  public static GameObjectState FromMessage(Message m)
  {
    var g = new GameObjectState();

    g.ApiPlayerId = m.GetInt();
    g.ClientId = m.GetUShort();
    g.RelayInstanceId = m.GetInt();
    g.NetworkInstanceId = m.GetInt();

    var fieldCount = m.GetInt();
    if (fieldCount > 0)
    {
      var fields = new List<RelayGameObjectFieldState>();
      for (int i = 0; i < fieldCount; i++)
      {
        var fieldHashcode = m.GetInt();
        var componentId = m.GetInt();
        var value = MessageSerializer.ReadObjectFromMessage(m);
        fields.Add(new RelayGameObjectFieldState()
        {
          FieldHashCode = fieldHashcode,
          Value = value,
          ComponentId = componentId
        });
      }
      g.State = fields.ToArray();
    }

    return g;
  }
}