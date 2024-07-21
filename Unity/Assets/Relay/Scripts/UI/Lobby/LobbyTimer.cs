using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class LobbyTimer : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI LobbyTimeTxtReference;

    private float startTime = 0.0f;

    private void OnEnable()
    {
      startTime = Time.time;
    }

    void Update()
    {
      LobbyTimeTxtReference.text = ((int)(Time.time - startTime)).ToString();
    }
  }
}