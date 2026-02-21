using System.Collections.Generic;
using UnityEngine;

public class ChooseGameMode : UnityMethodsSingleton<ChooseGameMode>
{
    [SerializeField] private GameModeSO[] gameModesSO;

    public GameModeSO[] GameModesSO { get { return gameModesSO; } }

    private Dictionary<GameModes, GameModeBase> gameModesDic = new();
    public GameModeBase CurrentGameMode { get; private set; }

    public override InitPriority Priority => InitPriority.ChooseGameMode;

    public override void OnInitAwake()
    {
        foreach (var gameMode in gameModesSO)
        {
            var mode = Instantiate(gameMode.modePrefab, transform);
            gameModesDic.Add(mode.GameModeSO.modeName, mode);
        }
    }

    public override void OnInitStart()
    {

    }

    public void SetCurrentGameMode(GameModes gameMode)
    {
        foreach(var mode in gameModesDic)
        {
            if(mode.Key == gameMode)
            {
                CurrentGameMode = mode.Value; 
                mode.Value.enabled = true;
            }
            else
            {
                mode.Value.enabled = false;
            }
        }

        ChooseGameModeUI.Instance.SetCurrentGameMode(gameMode);
    }

    public void StartPhase()
    {
        CurrentGameMode.StartMode();
    }
}
