using System;
using System.Collections.Generic;
using UnityEngine;

namespace BitterShark.Relay
{
  public class MessageBusManager : MonoBehaviour
  {
    private static MessageBusManager _instance;
    public static MessageBusManager Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;
    }

    private List<IMessageBusConsumer<object>> attachedConsumers = new List<IMessageBusConsumer<object>>();
    private List<Tuple<Type, object>> pendingMessages = new List<Tuple<Type, object>>();

    public void AttachConsumer(IMessageBusConsumer<object> consumer)
    {
      lock (attachedConsumers)
      {
        if (attachedConsumers.Contains(consumer))
        {
          Debug.LogError($"MessageBus: A consumer with the type name {consumer.GetType().Name} is already attached to the message bus");
        }
        else
        {
          attachedConsumers.Add(consumer);
        }
      }
    }

    public void DetachConsumer(IMessageBusConsumer<object> consumer)
    {
      lock (attachedConsumers)
      {
        if (attachedConsumers.Contains(consumer))
        {
          attachedConsumers.Remove(consumer);
        }
        else
        {
        }
      }
    }

    public void Publish<T>(T message)
    {
      lock (pendingMessages)
      {
        pendingMessages.Add(Tuple.Create(typeof(T), (object)message));
      }
    }

    private void Update()
    {
      lock (attachedConsumers)
      {
        lock (pendingMessages)
        {
          for (int i = 0; i < pendingMessages.Count; i++)
          {
            var message = pendingMessages[i];
            var handled = false;

            for (int x = 0; x < attachedConsumers.Count; x++)
            {
              if (attachedConsumers[x].MessageType == message.Item1)
              {
                try
                {
                  attachedConsumers[x].Consume(message.Item2);
                }
                catch (Exception e)
                {
                  Debug.Log($"Message could not be consumed {e}");
                }
              }
            }

            if (!handled)
            {
              //Debug.LogError($"MessageBus: No consumer is attached to handle a message of type {message.Item1.Name}. The message will go unhandled and lost.");
            }
          }

          pendingMessages.Clear();
        }
      }
    }
  }
}
