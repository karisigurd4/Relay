using UnityEngine;

namespace BitterShark.Relay
{
  public class TransitionOverlay : MonoBehaviour
  {
    [Header("Configuration")]
    [Range(0.1f, 4.0f)] public float TransitionSpeedMultiplier = 1.0f;
    public bool TransitionIn = false;

    private float transitionAmount = 0.0f;
    private bool transitionFinished = false;

    private void Start()
    {
    }

    private void Update()
    {
      transitionAmount = Mathf.Lerp(transitionAmount, TransitionIn ? 1.0f : 0.0f, Time.deltaTime * TransitionSpeedMultiplier);

      if (!transitionFinished)
      {
        if (TransitionIn)
        {
          if (transitionAmount > 0.99f)
          {
            MessageBusManager.Instance.Publish(new TransitionFinishedMessage() { });
            transitionFinished = true;
          }
        }
        else
        {
          if (transitionAmount < 0.01f)
          {
            MessageBusManager.Instance.Publish(new TransitionFinishedMessage() { });
            transitionFinished = true;
          }
        }
      }
    }
  }
}