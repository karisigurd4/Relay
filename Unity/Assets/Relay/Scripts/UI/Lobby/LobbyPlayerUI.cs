using TMPro;
using UnityEngine;

namespace BitterShark.Relay
{
  public class LobbyPlayerUI : MonoBehaviour
  {
    [Header("References")]
    public TextMeshProUGUI PlayerNameTextReference;

    public void Initialize(string playerName)
    {
      PlayerNameTextReference.text = playerName;
    }
  }
}