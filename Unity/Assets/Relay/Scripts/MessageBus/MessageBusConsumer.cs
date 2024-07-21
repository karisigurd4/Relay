using System;
using UnityEngine;

namespace BitterShark.Relay
{
  public abstract class MessageBusConsumer<T> : MonoBehaviour, IMessageBusConsumer<object>
  {
    public Type MessageType => messageType;

    public abstract void OnConsumeMessage(T message);

    private Type messageType = typeof(T);

    private void Start()
    {
      MessageBusManager.Instance.AttachConsumer(this);
    }

    private void OnDisable()
    {
      if (this != null)
      {
        MessageBusManager.Instance.DetachConsumer(this);
      }
    }

    public void Consume(object message)
    {
      var typedMessage = (T)message;
      OnConsumeMessage(typedMessage);
    }
  }
}