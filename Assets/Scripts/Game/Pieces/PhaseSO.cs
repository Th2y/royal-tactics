using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Start Pieces")]
public class PhaseSO : ScriptableObject
{
    public int phase = 1;
    public List<PieceDefinitionSO> availablePieces;
    public int startingPoints = 10;
    public int pointsAdvantageToWin = 5;
}
