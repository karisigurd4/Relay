using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class InformationDialog : MonoBehaviour
  {
    private static InformationDialog _instance;
    public static InformationDialog Instance
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

    [Header("References")]
    public TextMeshProUGUI InformationText;
    public TextMeshProUGUI HeaderLeftSideText;
    public TextMeshProUGUI HeaderRightSideText;

    public void Show(string information, string leftHeader, string rightHeader)
    {
      InformationText.text = information;
      HeaderLeftSideText.text = leftHeader;
      HeaderRightSideText.text = rightHeader;
    }
  }
}