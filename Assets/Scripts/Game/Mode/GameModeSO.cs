using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mode
{
    public string modeName;
    public string modeInfo;
    public string[] modeTutorial;
}

[CreateAssetMenu(fileName = "GameModeSO", menuName = "Royal Tactics/Game Mode")]
public class GameModeSO : ScriptableObject
{
    public GameModes modeName;
    public Mode modeTranslated;
    public GameModeBase modePrefab;
    public GameModeUIBase modeUIBase;
    public List<PhaseSO> phases;
}
