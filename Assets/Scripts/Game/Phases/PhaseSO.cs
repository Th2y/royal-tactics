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

    [Header("Generation Bias")]
    public int safeChance = 5;
    public int checkChance = 30;
    public int checkmateChance = 50;
    public int stalemateChance = 15;
}
