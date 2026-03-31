using UnityEngine;

public static class PlayerColorPrefs
{
    private const string Player1ColorKey = "PLAYER1_COLOR";
    private const string Player2ColorKey = "PLAYER2_COLOR";
    private const string BoardBorderColorKey = "BOARDBORDER_COLOR";

    private static string GetKeyByColorType(ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Player1 => Player1ColorKey,
            ColorType.Player2 => Player2ColorKey,
            ColorType.BoardBorder => BoardBorderColorKey,
            _ => BoardBorderColorKey,
        };
    }

    public static void SaveColor(Color color, ColorType colorType)
    {
        string colorKey = GetKeyByColorType(colorType);

        PlayerPrefs.SetFloat(colorKey + "_R", color.r);
        PlayerPrefs.SetFloat(colorKey + "_G", color.g);
        PlayerPrefs.SetFloat(colorKey + "_B", color.b);
        PlayerPrefs.SetFloat(colorKey + "_A", color.a);
        PlayerPrefs.Save();
    }

    public static Color LoadColor(Color fallback, ColorType colorType)
    {
        string colorKey = GetKeyByColorType(colorType);

        if (!PlayerPrefs.HasKey(colorKey + "_R")) return fallback;

        return new Color(
            PlayerPrefs.GetFloat(colorKey + "_R"),
            PlayerPrefs.GetFloat(colorKey + "_G"),
            PlayerPrefs.GetFloat(colorKey + "_B"),
            PlayerPrefs.GetFloat(colorKey + "_A")
        );
    }
}
