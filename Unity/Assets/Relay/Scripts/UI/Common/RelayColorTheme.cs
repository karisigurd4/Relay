using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BitterShark.Relay
{
  public class RelayColorTheme : MonoBehaviour
  {
    [Header("Color theme to apply to the object")]
    public RelayUIColorTheme ColorTheme;
    [Space(10)]
    [Header("Type of object")]
    public RelayColorThemeType ColorThemeType;

    private Image image = null;
    private static Dictionary<RelayColorThemeType, Color> typeToColorMap = null;

    private void OnValidate()
    {
      if (image == null)
      {
        image = GetComponent<Image>();
      }

      typeToColorMap = new Dictionary<RelayColorThemeType, Color>()
    {
      { RelayColorThemeType.Background, ColorTheme.BackgroundColor },
      { RelayColorThemeType.Subsection, ColorTheme.SubsectionColor },
      { RelayColorThemeType.Dropdown, ColorTheme.DropdownColor },
      { RelayColorThemeType.Divider, ColorTheme.DividerColor },
      { RelayColorThemeType.TitleBgr, ColorTheme.TitleBgrColor },
      { RelayColorThemeType.DropdownTitle, ColorTheme.DropdownTitleColor },
    };

      image.color = typeToColorMap[ColorThemeType];
    }
  }
}