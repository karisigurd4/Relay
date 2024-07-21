using UnityEngine;

namespace BitterShark.Relay
{
  public class RelaySceneLoader : MonoBehaviour
  {
    [Header("Scene configurations")]
    public RelaySceneConfiguration SceneConfiguration;

    private void Start()
    {
      if (RelaySceneManager.Instance != null)
      {
        RelaySceneManager.Instance.LoadSceneConfiguration(SceneConfiguration);
      }
    }
  }
}