using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class Tutorial
{
    public LocalizedString tutorialDescriptionL;
    public LocalizedSprite tutorialSpriteL;
}

[System.Serializable]
public class Mode
{
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
