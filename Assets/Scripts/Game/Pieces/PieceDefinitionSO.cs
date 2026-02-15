using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Piece Definition")]
public class PieceDefinitionSO : ScriptableObject
{
    public PieceType type;

    [Header("Gameplay")]
    public int cost;

    [Header("Prefab")]
    public Piece prefab;

    [Header("Progression")]
    public bool unlockedByDefault = true;
    public int winsRequiredToUnlock;
}
