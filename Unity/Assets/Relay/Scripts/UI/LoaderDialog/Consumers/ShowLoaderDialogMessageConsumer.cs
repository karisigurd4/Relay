using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowLoaderDialogMessageConsumer : MessageBusConsumer<ShowLoaderDialogMessage>
  {
    [Header("References")]
    public LoaderDialog LoaderDialog;

    public override void OnConsumeMessage(ShowLoaderDialogMessage message)
    {
      LoaderDialog.gameObject.SetActive(true);
      LoaderDialog.SetLoadingText(message.LoadingText);
    }
  }
}
