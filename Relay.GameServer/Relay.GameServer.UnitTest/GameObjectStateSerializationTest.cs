using NUnit.Framework;
using Relay.GameServer.Contracts;
using Riptide;

namespace Relay.GameServer.UnitTest
{
  public class GameObjectStateSerializationTest
  {
    [Test]
    public void Can_Serialize_Null_State()
    {
      var g = new GameObjectState()
      {
        ApiPlayerId = 18,
        ClientId = 12,
        NetworkInstanceId = 10,
        RelayInstanceId = 9
      };

      var m = Message.Create(MessageSendMode.Unreliable, (ushort)GameServerMessageType.ClientGameState);

      g.AppendToMessage(m);

      for (int i = 0; i < 2; i++)
      {
        var b = m.GetByte();
      }

      var g2 = GameObjectState.FromMessage(m);

      Assert.AreEqual(g2.ApiPlayerId, 18);
      Assert.AreEqual(g2.ClientId, 12);
      Assert.AreEqual(g2.NetworkInstanceId, 10);
      Assert.AreEqual(g2.RelayInstanceId, 9);
      Assert.IsNull(g2.State);
    }

    [Test]
    public void Can_Serialize_Not_Null_State()
    {
      var g = new GameObjectState()
      {
        ApiPlayerId = 18,
        ClientId = 12,
        NetworkInstanceId = 10,
        RelayInstanceId = 9,
        State = new Core.RelayGameObjectFieldState[] {
          new Core.RelayGameObjectFieldState() {
            ComponentId = 1,
            FieldHashCode = 2,
            Value = 3
          },
          new Core.RelayGameObjectFieldState() {
            ComponentId = 4,
            FieldHashCode = 5,
            Value = "Hello world!"
          }
        }
      };

      var m = Message.Create(MessageSendMode.Unreliable, (ushort)GameServerMessageType.ClientGameState);

      g.AppendToMessage(m);

      for (int i = 0; i < 2; i++)
      {
        var b = m.GetByte();
      }

      var g2 = GameObjectState.FromMessage(m);

      Assert.AreEqual(g2.ApiPlayerId, 18);
      Assert.AreEqual(g2.ClientId, 12);
      Assert.AreEqual(g2.NetworkInstanceId, 10);
      Assert.AreEqual(g2.RelayInstanceId, 9);
      Assert.AreEqual(g2.State.Length, 2);
      Assert.AreEqual(g2.State[0].ComponentId, 1);
      Assert.AreEqual(g2.State[0].FieldHashCode, 2);
      Assert.AreEqual(g2.State[0].Value, 3);
      Assert.AreEqual(g2.State[1].ComponentId, 4);
      Assert.AreEqual(g2.State[1].FieldHashCode, 5);
      Assert.AreEqual(g2.State[1].Value, "Hello world!");
    }
  }
}