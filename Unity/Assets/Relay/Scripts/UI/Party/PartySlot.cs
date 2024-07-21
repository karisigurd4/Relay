using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class PartySlot : MonoBehaviour
  {
    [Header("References")]
    public Image PlayerProfileImageReference;
    public TextMeshProUGUI PlayerNameTextReference;
    public TextMeshProUGUI PartyLeaderTextReference;

    private bool populated = false;

    public void Populate(Sprite playerProfileSprite, string playerName, bool isPartyLeader)
    {
      PlayerProfileImageReference.sprite = playerProfileSprite;
      PlayerNameTextReference.text = playerName;
      populated = true;
      PartyLeaderTextReference.gameObject.SetActive(isPartyLeader);
    }

    public void Clear()
    {
      PlayerProfileImageReference.sprite = null;
      PlayerNameTextReference.text = string.Empty;
      populated = false;
    }

    public void OnClick()
    {
      if (!populated)
      {
        MessageBusManager.Instance.Publish(new ShowFriendsWindowMessage()
        {
        });
      }
    }
  }
}
