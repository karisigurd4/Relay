using UnityEngine;

namespace BitterShark.Relay
{
  public class HideLoaderDialogMessageConsumer : MessageBusConsumer<HideLoaderDialogMessage>
  {
    [Header("References")]
    public LoaderUI LoaderUI;
    public GameObject LoaderDialog;

    public override void OnConsumeMessage(HideLoaderDialogMessage message)
    {
      LoaderUI.StopLoading();
      LoaderDialog.SetActive(false);
    }
  }
}
