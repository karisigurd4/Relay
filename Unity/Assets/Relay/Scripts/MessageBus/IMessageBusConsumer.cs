using System;

namespace BitterShark.Relay
{
  public interface IMessageBusConsumer<T>
  {
    void Consume(T message);
    Type MessageType { get; }
  }
}