using UnityEngine;

namespace BitterShark.Relay
{
  public class ApiConfiguration : MonoBehaviour
  {
    private static ApiConfiguration _instance;
    public static ApiConfiguration Instance
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

    public string ApiRoute = $"https://127.0.0.1:5000";
  }
}
