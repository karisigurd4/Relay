using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class FindingGameServerWindow : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI CounterTextReference;

    private float timeActive = 0.0f;

    private void Start()
    {
      timeActive = 0.0f;
    }

    private void Update()
    {
      timeActive += Time.deltaTime;

      CounterTextReference.text = $"{((int)timeActive)}";
    }
  }
}