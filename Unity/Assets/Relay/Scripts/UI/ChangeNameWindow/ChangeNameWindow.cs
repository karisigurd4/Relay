using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class ChangeNameWindow : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI NameTextReference;

    public void SetPlayerName()
    {
      PlayerApiClient.Instance.SetPlayerName(RelayPlayerManager.Instance.GetPlayerApiKey(), NameTextReference.text, response =>
      {
        if (response.Success)
        {
          MessageBusManager.Instance.Publish(new PlayerNameChangedMessage()
          {
            NewPlayerName = NameTextReference.text
          });

          gameObject.SetActive(false);
        }
        else
        {
          MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
          {
            InformationData = response.Message,
            LeftHeaderText = "Error",
            RightHeaderText = ""
          });
        }
      });
    }
  }
}