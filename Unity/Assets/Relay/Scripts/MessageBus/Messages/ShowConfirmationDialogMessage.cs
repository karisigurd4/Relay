using System;

namespace BitterShark.Relay
{
  public class ShowConfirmationDialogMessage
  {
    public string Text { get; set; }
    public Action OnConfirmAction { get; set; }
  }
}