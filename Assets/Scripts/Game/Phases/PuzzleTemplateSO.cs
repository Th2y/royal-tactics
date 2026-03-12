using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzlePiece
{
    [Header("Piece")]
    public List<PieceDefinitionSO> pieceOptions;
    public bool isPlayer;

    [Header("Placement")]
    public PositionConstraint constraint = PositionConstraint.Fixed;
    public Vector2Int position;

    [Header("Spawn")]
    [Range(0, 100)]
    public int spawnChance = 100;
}

[CreateAssetMenu(menuName = "Royal Tactics/Puzzle Template")]
public class PuzzleTemplateSO : ScriptableObject
{
    public KingState state;

    public List<PuzzlePiece> pieces;
}
