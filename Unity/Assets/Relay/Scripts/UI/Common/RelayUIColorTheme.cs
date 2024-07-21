using UnityEngine;

namespace BitterShark.Relay
{
  [CreateAssetMenu(fileName = "UIColorTheme", menuName = "Relay/UI Color Theme")]
  public class RelayUIColorTheme : ScriptableObject
  {
    public Color BackgroundColor;
    public Color SubsectionColor;
    public Color DividerColor;
    public Color DropdownColor;
    public Color TitleBgrColor;
    public Color DropdownTitleColor;
  }
}