using System.Collections.Generic;
using System.Linq;

namespace BitterShark.Relay
{
  public class ThreadSafeQueue<T>
  {
    private List<T> queue = new List<T>();

    public int Count => queue.Count;

    public void Push(T obj)
    {
      lock (queue)
      {
        queue.Add(obj);
      }
    }

    public T Pop()
    {
      lock (queue)
      {
        var item = queue.LastOrDefault();

        if (queue.Count > 0)
        {
          queue.RemoveAt(queue.Count - 1);
        }

        return item;
      }
    }

    public T[] PopAll()
    {
      lock (queue)
      {
        var items = queue.ToArray();

        queue.Clear();

        return items;
      }
    }
  }
}
