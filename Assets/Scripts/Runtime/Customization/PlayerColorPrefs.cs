using UnityEngine;

public static class PlayerColorPrefs
{
    private const string PlayerColorKey = "PLAYER_COLOR";
    private const string OponentColorKey = "OPONENT_COLOR";

    public static void SaveColor(Color color, bool isPlayer)
    {
        string colorKey = isPlayer ? PlayerColorKey : OponentColorKey;

        PlayerPrefs.SetFloat(colorKey + "_R", color.r);
        PlayerPrefs.SetFloat(colorKey + "_G", color.g);
        PlayerPrefs.SetFloat(colorKey + "_B", color.b);
        PlayerPrefs.SetFloat(colorKey + "_A", color.a);
        PlayerPrefs.Save();
    }

    public static Color LoadColor(Color fallback, bool isPlayer)
    {
        string colorKey = isPlayer ? PlayerColorKey : OponentColorKey;

        if (!PlayerPrefs.HasKey(colorKey + "_R")) return fallback;

        return new Color(
            PlayerPrefs.GetFloat(colorKey + "_R"),
            PlayerPrefs.GetFloat(colorKey + "_G"),
            PlayerPrefs.GetFloat(colorKey + "_B"),
            PlayerPrefs.GetFloat(colorKey + "_A")
        );
    }
}
