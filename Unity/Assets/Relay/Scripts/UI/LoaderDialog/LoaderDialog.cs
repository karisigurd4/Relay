using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class LoaderDialog : MonoBehaviour
  {
    [Header("References")]
    public LoaderUI LoaderUI;
    public TextMeshProUGUI LoadingText;

    private void OnEnable()
    {
      LoaderUI.StartLoading();
    }

    public void SetLoadingText(string text)
    {
      LoadingText.text = text;
    }
  }
}