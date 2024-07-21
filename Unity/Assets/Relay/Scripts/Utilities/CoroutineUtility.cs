using System;
using System.Collections;
using UnityEngine;

namespace BitterShark.Relay
{
  public class CoroutineUtility : MonoBehaviour
  {
    private static CoroutineUtility _instance;
    public static CoroutineUtility Instance
    {
      get => _instance;
      private set
      {
        if (_instance == null)
        {
          _instance = value;
        }
        else
        {
          Debug.LogError("Instance already set");
          Destroy(value);
        }
      }
    }

    public void Awake()
    {
      _instance = this;
    }

    public void StartCoroutine(float delay, Action action)
    {
      StartCoroutine(Coroutine(delay, action));
    }

    private IEnumerator Coroutine(float delay, Action action)
    {
      yield return new WaitForSeconds(delay);
      action();
    }
  }
}
