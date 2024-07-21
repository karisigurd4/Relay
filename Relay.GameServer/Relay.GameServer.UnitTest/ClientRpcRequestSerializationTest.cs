using NUnit.Framework;
using Relay.GameServer.Contracts;
using Riptide;

namespace Relay.GameServer.UnitTest
{
  public class ClientRpcRequestSerializationTest
  {
    [Test]
    public void Can_Serialize_Null_Parameter_RPC()
    {
      var r = new ClientRpcRequest()
      {
        Broadcast = false,
        ComponentId = 10,
        MethodId = 11,
        Parameters = null,
        RelayGameObjectId = 2,
        SenderClientId = 3
      };

      var m = Message.Create(MessageSendMode.Unreliable, (ushort)GameServerMessageType.ClientRpcRequest);

      r.AppendToMessage(m);

      for (int i = 0; i < 2; i++)
      {
        var b = m.GetByte();
      }

      var f = ClientRpcRequest.FromMessage(m);

      Assert.AreEqual(f.Broadcast, false);
      Assert.AreEqual(f.ComponentId, 10);
      Assert.AreEqual(f.MethodId, 11);
      Assert.AreEqual(f.Parameters, null);
      Assert.AreEqual(f.RelayGameObjectId, 2);
      Assert.AreEqual(f.SenderClientId, 3);

    }
  }
}
