using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class PlayerProfileCard : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI ActivityStatusText;
    public Image PlayerProfileImage;
    public GameObject DropdownMenu;
    public bool Clickable = true;
    public GameObject ThreeDotsButton;

    private int playerId;

    public void Initialize(int playerId, string playerName, bool activityStatus, Sprite playerProfileSprite)
    {
      this.playerId = playerId;
      if (ThreeDotsButton != null)
      {
        ThreeDotsButton.SetActive(playerId == RelayPlayerManager.Instance.GetPlayerId());
      }
      DropdownMenu.SetActive(false);
      PlayerNameText.text = playerName;
      ActivityStatusText.color = activityStatus ? new Color(0, 255, 0) : new Color(168, 168, 168);
      ActivityStatusText.text = activityStatus ? "Online" : "Offline";
      //PlayerProfileImage.sprite = playerProfileSprite;
    }

    public void OnClick()
    {
      DropdownMenu.SetActive(false);

      if (Clickable)
      {
        MessageBusManager.Instance.Publish(new ShowPlayerProfileWindowMessage()
        {
          PlayerId = playerId
        });
      }
    }

    public void SendPartyInvite()
    {
      DropdownMenu.SetActive(false);
      MessageBusManager.Instance.Publish(new ShowConfirmationDialogMessage()
      {
        Text = $"Send a party invite to {PlayerNameText.text}",
        OnConfirmAction = () =>
        {
          PartyApiClient.Instance.InvitePlayerToParty(RelayProjectSettings.GetProjectSettings().ProjectId, RelayPlayerManager.Instance.GetPlayerApiKey(), playerId, response =>
          {
            if (!response.Success)
            {
              MessageBusManager.Instance.Publish(new ShowInformationDialogMessage()
              {
                InformationData = response.Message,
                LeftHeaderText = string.Empty,
                RightHeaderText = string.Empty
              });
            }
          });
        }
      });
    }

    public void RemoveFriend()
    {
      DropdownMenu.SetActive(false);
      MessageBusManager.Instance.Publish(new ShowConfirmationDialogMessage()
      {
        Text = $"Remove {PlayerNameText.text} from your friend list?",
        OnConfirmAction = () =>
        {
          PlayerApiClient.Instance.RemovePlayerFromFriendList(RelayPlayerManager.Instance.GetPlayerApiKey(), playerId, response =>
          {
            MessageBusManager.Instance.Publish(new FriendListUpdatedMessage()
            {
            });
          });
        }
      });
    }
  }
}
