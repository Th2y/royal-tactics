using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Colors")]
public class ColorsOptions : ScriptableObject
{
    public Color onSelectedPieceColor;

    [Header("Pieces")]
    public Color[] piecesColors;
    [Tooltip("Between 0 and colors length and different by index of player 2")]
    public int defaultColorIndexPlayer1 = 0;
    [Tooltip("Between 0 and colors length and different by index of player 1")]
    public int defaultColorIndexPlayer2 = 1;

    [Header("Board")]
    public Color[] boardBorderColors;

    private void OnValidate()
    {
        if (piecesColors == null || piecesColors.Length == 0) return;

        // Ensures that the indices are within the range
        defaultColorIndexPlayer1 = Mathf.Clamp(defaultColorIndexPlayer1, 0, piecesColors.Length - 1);
        defaultColorIndexPlayer2 = Mathf.Clamp(defaultColorIndexPlayer2, 0, piecesColors.Length - 1);

        // Make sure they are different
        if (defaultColorIndexPlayer1 == defaultColorIndexPlayer2)
        {
            defaultColorIndexPlayer2 = (defaultColorIndexPlayer1 + 1) % piecesColors.Length;
        }
    }
}
