using UnityEngine;

namespace BitterShark.Relay
{
  public class ShowInformationDialogMessageConsumer : MessageBusConsumer<ShowInformationDialogMessage>
  {
    [Header("References")]
    public GameObject InformationDialogReference;

    public override void OnConsumeMessage(ShowInformationDialogMessage message)
    {
      InformationDialogReference.SetActive(true);
      InformationDialog.Instance.Show(message.InformationData, message.LeftHeaderText, message.RightHeaderText);
    }
  }
}