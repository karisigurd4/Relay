using UnityEngine;

namespace BitterShark.Relay
{
  public class PlayerProfileWindow : MonoBehaviour
  {
    [Header("References")]
    public PlayerProfileCard PlayerProfileCard;
    public GameObject DropDownMenu;

    public void Show(int playerId)
    {
      PlayerApiClient.Instance.GetPlayer(RelayProjectSettings.GetProjectSettings().ProjectId, playerId, null, response =>
      {
        PlayerProfileCard.Initialize(response.Player.Id, response.Player.Name, response.Player.PlayerActive, null);
      });
    }

    public void OpenChangePlayerNameWindow()
    {
      MessageBusManager.Instance.Publish(new ShowChangeNameWindowMessage()
      {
      });
    }

    public void ToggleDropdownMenu()
    {
      DropDownMenu.SetActive(!DropDownMenu.activeInHierarchy);
    }
  }
}