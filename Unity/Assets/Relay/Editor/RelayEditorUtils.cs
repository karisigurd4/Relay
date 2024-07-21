using UnityEditor;
using UnityEngine;

namespace BitterShark.Relay
{
  public static class RelayEditorUtils
  {
    public static GUIStyle HeaderFont()
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 12;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.normal.textColor = Color.white;
      return headerStyle;
    }

    public static GUIStyle CenterHeaderFont()
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 12;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.normal.textColor = Color.white;
      headerStyle.alignment = TextAnchor.MiddleCenter;
      return headerStyle;
    }

    public static GUIStyle HeaderRowFont()
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 11;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.normal.textColor = Color.white;
      return headerStyle;
    }


    public static GUIStyle BoldRedFont(TextAnchor textAnchor = TextAnchor.MiddleCenter)
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 11;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.normal.textColor = Color.red;
      headerStyle.alignment = textAnchor;
      return headerStyle;
    }


    public static GUIStyle FieldNameFont(bool active, TextAnchor textAnchor = TextAnchor.UpperLeft, int fontSize = 12)
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = fontSize;
      headerStyle.normal.textColor = active ? Color.white : Color.grey;
      headerStyle.alignment = textAnchor;
      return headerStyle;
    }

    public static GUIStyle RequiredFont()
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 10;
      headerStyle.alignment = TextAnchor.MiddleRight;
      headerStyle.contentOffset = new Vector2(0, 5);
      headerStyle.padding = new RectOffset(0, 10, 0, 0);
      headerStyle.normal.textColor = Color.yellow;
      return headerStyle;
    }

    public static GUIStyle SmallInfoFont()
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 10;
      headerStyle.alignment = TextAnchor.MiddleLeft;
      headerStyle.contentOffset = new Vector2(0, 5);
      headerStyle.padding = new RectOffset(0, 10, 0, 0);
      headerStyle.normal.textColor = Color.white;
      return headerStyle;
    }

    public static GUIStyle StateFoldoutFont(bool active)
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 13;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.alignment = TextAnchor.MiddleLeft;
      headerStyle.normal.textColor = active ? Color.white : new Color(0.65f, 0.65f, 0.65f, 1.0f);
      return headerStyle;
    }

    public static GUIStyle AlignMiddleCenter()
    {
      var style = new GUIStyle();
      style.alignment = TextAnchor.MiddleCenter;
      return style;
    }

    public static GUIStyle FoldoutFont(bool active)
    {
      var headerStyle = new GUIStyle();
      headerStyle.fontSize = 12;
      headerStyle.fontStyle = FontStyle.Bold;
      headerStyle.normal.textColor = active ? Color.white : new Color(0.65f, 0.65f, 0.65f, 1.0f);
      return headerStyle;
    }

    public static void BottonPad(int height = 1)
    {
      var topPadding = EditorGUILayout.GetControlRect(false, height);
      topPadding.height = height;
      EditorGUI.DrawRect(topPadding, Color.clear);
    }

    public static void GuiLine(int height = 1)
    {
      Rect rect = EditorGUILayout.GetControlRect(false, height);
      rect.height = height;
      var topPadding = EditorGUILayout.GetControlRect(false, 2);
      topPadding.height = 2;
      EditorGUI.DrawRect(topPadding, Color.clear);
      EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    public static GUIStyle LabelFont()
    {
      var style = new GUIStyle();
      style.normal.textColor = Color.white;
      return style;
    }

    public static GUIStyle CenterLabelFont()
    {
      var style = new GUIStyle();
      style.normal.textColor = Color.grey;
      style.wordWrap = true;
      style.alignment = TextAnchor.MiddleCenter;
      return style;
    }

    public static GUIStyle NoteLabel()
    {
      GUIStyle textStyle = EditorStyles.label;
      textStyle.wordWrap = true;
      return textStyle;
    }

    public static void RowHeaderGuiLine(int height = 1)
    {
      Rect rect = EditorGUILayout.GetControlRect(false, height);
      rect.height = height;
      EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    public static Texture2D MakeColorTexture()
    {
      Color[] pix = new Color[600 * 1];

      for (int z = 0; z < pix.Length; z++)
        pix[z] = new Color(.1f, .1f, .1f, 1);

      Texture2D result = new Texture2D(600, 1);
      result.SetPixels(pix);
      result.Apply();

      GUIStyle gsAlterQuest = new GUIStyle();
      gsAlterQuest.normal.background = result;

      return result;
    }

    public static string TruncateWithEllipsis(string input, int maxLength)
    {
      if (string.IsNullOrEmpty(input))
        return input;

      if (input.Length <= maxLength)
        return input;

      return input.Substring(0, maxLength) + "...";
    }
  }
}