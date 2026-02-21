using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeUI : UnityMethodsSingleton<ChooseGameModeUI>
{
    [Header("Mode Scripts")]
    [SerializeField] private GameModeUIBase[] modes;

    [Header("Mode")]
    [SerializeField] private Transform modeCardParent;
    [SerializeField] private ChooseGameModeCard modeCardPrefab;

    [Header("Phase")]
    [SerializeField] private Transform phaseModeParent;
    [SerializeField] private CanvasGroupController phaseModePrefab;
    [SerializeField] private ChooseGamePhaseCard phasePrefab;

    [Header("References for Game Mode UI")]
    public TextMeshProUGUI GameTurnText;
    public TextMeshProUGUI GameOverText;
    public CanvasGroupController GameTurnParent;
    public CanvasGroupController AdvantageParent;
    public CanvasGroupController PlacementParent;
    public CanvasGroupController PieceParent;
    public CanvasGroupController TileParent;
    public CanvasGroupController PromotionParent;
    public Button FinishBtn;
    public TextMeshProUGUI FinishBtnTxt;

    public Dictionary<int, CanvasGroupController> PhasesModesParent { get; private set; } = new();
    private List<ChooseGamePhaseCard> phases = new();

    private Dictionary<GameModes, GameModeUIBase> gameModesDic = new();
    public GameModeUIBase CurrentGameMode { get; private set; }

    public override InitPriority Priority => InitPriority.ChooseGameModeUI;

    public override void OnInitAwake()
    {
        SetScreens();

        var choose = ChooseGameMode.Instance;
        foreach(var mode in choose.GameModesSO)
        {
            var modeCard = Instantiate(modeCardPrefab, modeCardParent);
            var phaseMode = Instantiate(phaseModePrefab, phaseModeParent);
            PhasesModesParent.Add((int)mode.modeName, phaseMode);

            modeCard.Init(mode);

            foreach (var phase in mode.phases)
            {
                var phaseO = Instantiate(phasePrefab, phaseMode.transform);
                phaseO.Init(mode, phase);
                phases.Add(phaseO);
            }
        }

        foreach(var mode in modes)
        {
            gameModesDic.Add(mode.GameModeSO.modeName, mode);
        }
    }

    public override void OnInitStart()
    {
        
    }

    public void SetCurrentGameMode(GameModes gameMode)
    {
        foreach (var mode in gameModesDic)
        {
            if (mode.Key == gameMode)
            {
                CurrentGameMode = mode.Value;
                mode.Value.enabled = true;
            }
            else
            {
                mode.Value.enabled = false;
            }
        }
    }

    public void ReloadPhases()
    {
        foreach(var phase in phases)
        {
            phase.CheckUnlocked();
        }
    }

    #region Game Screen
    private Dictionary<GameScreen, CanvasGroupController> _screenMap;

    private void SetScreens()
    {
        CanvasGroupController[] screens = FindObjectsByType<CanvasGroupController>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        _screenMap = new Dictionary<GameScreen, CanvasGroupController>();

        foreach (var screen in screens)
        {
            if(screen.GameScreen != GameScreen.None)
            {
                _screenMap.Add(screen.GameScreen, screen);
                screen.SetActive(screen.GameScreen == GameScreen.ChooseGameMode);
            }
        }
    }

    public void ShowScreen(GameScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            view.SetActive(view.GameScreen == screen);
        }
    }

    public void HideScreen(GameScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            if (view.GameScreen == screen)
            {
                view.SetActive(false);
            }
        }
    }
    #endregion
}
