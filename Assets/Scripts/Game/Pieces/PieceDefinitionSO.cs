using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Piece Definition")]
public class PieceDefinitionSO : ScriptableObject
{
    public PieceType type;
    public string namePt;

    public List<TileName> humanInitialTiles;
    public List<TileName> iaInitialTiles;

    [Header("Gameplay")]
    public int cost;

    [Header("Prefab")]
    public Piece prefab;
}
