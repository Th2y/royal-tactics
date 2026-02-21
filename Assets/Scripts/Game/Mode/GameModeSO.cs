using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeSO", menuName = "Royal Tactics/Game Mode")]
public class GameModeSO : ScriptableObject
{
    public int modeId;
    public GameModes modeName;
    public string modeNamePt;
    public GameModeBase modePrefab;
    public GameModeUIBase modeUIBase;
    public List<PhaseSO> phases;
}
