using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class PlayerProfileSearchResult : MonoBehaviour
  {
    [Header("References")]
    public Image ProfileImage;
    public TextMeshProUGUI ProfileNameText;

    private int playerId;

    public void Initialize(Sprite profileImageSprite, string playerName, int playerId)
    {
      ProfileImage.sprite = profileImageSprite;
      ProfileNameText.text = playerName;
      this.playerId = playerId;
    }

    public void OnClick()
    {
      MessageBusManager.Instance.Publish(new ShowConfirmationDialogMessage()
      {
        OnConfirmAction = () =>
        {
          PlayerApiClient.Instance.SendPlayerFriendRequest(RelayProjectSettings.GetProjectSettings().ProjectId, RelayPlayerManager.Instance.GetPlayerApiKey(), playerId, response =>
          {
            if (!response.Success)
            {
              MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
              {
                InformationData = response.Message,
                LeftHeaderText = "",
                RightHeaderText = ""
              });
            }
          });
        },
        Text = $"Do you want to send a friend request to {ProfileNameText.text}"
      });
    }
  }
}
