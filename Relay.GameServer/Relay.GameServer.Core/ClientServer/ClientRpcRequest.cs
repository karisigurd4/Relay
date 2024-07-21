using Riptide;

public class ClientRpcRequest
{
  public ushort SenderClientId;
  public bool Broadcast;
  public ushort ReceiverClientId;
  public int RelayGameObjectId;
  public int ComponentId;
  public int MethodId;
  public object[] Parameters;

  public Message AppendToMessage(Message m)
  {
    m.AddUShort(SenderClientId);
    m.AddBool(Broadcast);
    if (!Broadcast)
    {
      m.AddUShort(ReceiverClientId);
    }
    m.AddInt(RelayGameObjectId);
    m.AddInt(ComponentId);
    m.AddInt(MethodId);
    MessageSerializer.WriteParametersToMessage(m, Parameters ?? new object[] { });
    return m;
  }

  public static ClientRpcRequest FromMessage(Message m)
  {
    var r = new ClientRpcRequest();
    r.SenderClientId = m.GetUShort();
    r.Broadcast = m.GetBool();
    if (!r.Broadcast)
    {
      r.ReceiverClientId = m.GetUShort();
    }
    r.RelayGameObjectId = m.GetInt();
    r.ComponentId = m.GetInt();
    r.MethodId = m.GetInt();
    r.Parameters = MessageSerializer.ReadParametersFromMessage(m);
    return r;
  }
}
