using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tutorial
{
    public string tutorialDescription;
    public Sprite tutorialSprite;
}

[System.Serializable]
public class Mode
{
    public string modeName;
    public string modeInfo;
    public Tutorial[] modeTutorial;
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
