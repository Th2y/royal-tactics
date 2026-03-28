using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Royal Tactics/Piece Definition")]
public class PieceDefinitionSO : ScriptableObject
{
    public PieceType type;
    public LocalizedString nameT;

    public List<TileName> humanInitialTiles;
    public List<TileName> aiInitialTiles;

    [Header("Gameplay")]
    public int cost;

    [Header("Prefab")]
    public Piece prefab;
}
