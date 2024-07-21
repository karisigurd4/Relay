using UnityEngine;

namespace BitterShark.Relay
{
  public class GameServerConnectionPlayerBase : MonoBehaviour
  {
    [Header("Prefabs")]
    public GameObject PlayerBase;

    private bool spawned = false;

    private void Update()
    {
      if (GameServerClient.IsConnected && !spawned)
      {
        Instantiate(PlayerBase);
        spawned = true;
        Destroy(gameObject);
      }
    }
  }
}
