using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Piece Definition")]
public class PieceDefinitionSO : ScriptableObject
{
    public PieceType type;
    public string namePt;

    [Header("Gameplay")]
    public int cost;

    [Header("Prefab")]
    public Piece prefab;
}
