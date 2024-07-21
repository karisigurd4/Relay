using UnityEngine;

namespace BitterShark.Relay
{
  public class RelayAnimateUIProperty : MonoBehaviour
  {
    [Header("General configuration")]
    public float AnimationEventTimeMultiplier = 1.0f;

    [Space(10)]
    [Header("Animation properties")]
    public bool AnimateScale = false;
    public AnimationCurve ScaleAnimationCurve;

    [Space(10)]
    [Header("Play options")]
    public bool PlayOnEnable = true;

    private bool playing = false;
    private float timeCoefficient = 0.0f;

    private void OnEnable()
    {
      Play();
    }

    private void Play()
    {
      playing = true;
      timeCoefficient = 0.0f;
    }

    private void Update()
    {
      if (playing)
      {
        if (AnimateScale)
        {
          timeCoefficient += Time.deltaTime * AnimationEventTimeMultiplier;
          transform.localScale = Vector3.one * ScaleAnimationCurve.Evaluate(timeCoefficient);
        }

        if (timeCoefficient > 1.0f)
        {
          playing = false;
        }
      }
    }
  }
}
