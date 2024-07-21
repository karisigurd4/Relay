using UnityEngine;

namespace BitterShark.Relay
{
  public class ServerBrowserButton : MonoBehaviour
  {
    public void ShowGameServerBrowserWindow()
    {
      MessageBusManager.Instance.Publish(new ShowGameServerBrowserWindowMessage()
      {

      });
    }
  }
}
