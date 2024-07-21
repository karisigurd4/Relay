using System;
using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class ConfirmationDialog : MonoBehaviour
  {
    public TextMeshProUGUI ConfirmationDialogText;

    private Action onConfirmAction;

    public void Show(string text, Action action)
    {
      ConfirmationDialogText.text = text;
      onConfirmAction = action;
    }

    public void Confirm()
    {
      onConfirmAction();
      onConfirmAction = null;
      gameObject.SetActive(false);
    }
  }
}
