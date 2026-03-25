using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[System.Serializable]
public class Tutorial
{
    public string tutorialDescription;
    public Sprite tutorialSprite;
    public LocalizedString tutorialDescriptionL;
    public LocalizedSprite tutorialSpriteL;
}

[System.Serializable]
public class Mode
{
    public string modeInfo;
    public LocalizedString modeNameL;
    public LocalizedString modeInfoL;
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
