namespace Relay.GameServer.Implementation.Interfaces
{
  using Riptide;

  public interface IMessageSerializer
  {
    Message Serialize<T>(T obj);
    T Deserialize<T>(Message message);
  }
}
