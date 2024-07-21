using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowConfirmationDialogMessageConsumer : MessageBusConsumer<ShowConfirmationDialogMessage>
  {
    [Header("Reference")]
    public ConfirmationDialog ConfirmationDialogReference;

    public override void OnConsumeMessage(ShowConfirmationDialogMessage message)
    {
      ConfirmationDialogReference.Show(message.Text, message.OnConfirmAction);
      ConfirmationDialogReference.gameObject.SetActive(true);
    }
  }
}