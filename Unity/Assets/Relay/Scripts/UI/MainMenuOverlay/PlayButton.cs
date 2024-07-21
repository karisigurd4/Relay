using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class PlayButton : MonoBehaviour
  {
    [Header("Scene configurations")]
    public RelaySceneConfiguration TransitionInSceneConfiguration;

    [Header("References")]
    public Image ButtonBgrImageReference;

    public void PressDown()
    {
      ButtonBgrImageReference.color = Color.white * 0.8f;
    }

    public void PressUp()
    {
      ButtonBgrImageReference.color = Color.white;
      RelaySceneManager.Instance.LoadSceneConfiguration(TransitionInSceneConfiguration);
    }
  }
}