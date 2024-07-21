using UnityEngine;

namespace BitterShark.Relay
{
  [RequireComponent(typeof(CanvasGroup))]
  public class CanvasGroupManager : MonoBehaviour
  {
    [Header("Configuration")]
    [Range(0.1f, 8.0f)] public float AlphaUpdateTimeMultiplier = 1.0f;
    public bool On = true;

    private CanvasGroup canvasGroup;

    void Start()
    {
      canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
      canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, (On ? 1.0f : 0.0f), Time.deltaTime * AlphaUpdateTimeMultiplier);
    }
  }
}