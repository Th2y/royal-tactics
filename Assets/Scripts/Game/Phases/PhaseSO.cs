using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Royal Tactics/Phase")]
public class PhaseSO : ScriptableObject
{
    public int phase = 1;
    public List<PieceDefinitionSO> availablePiecesHuman;
    public List<PieceDefinitionSO> availablePiecesAI;
    public List<PieceDefinitionSO> availablePiecesPromotion;
    public List<TileName> tilesNames;
    public int startingPoints = 10;
    public int pointsAdvantageToWin = 5;
}
