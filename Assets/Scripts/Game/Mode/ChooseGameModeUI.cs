using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ChooseGameModeUI : UnityMethodsSingleton<ChooseGameModeUI>
{
    [SerializeField] private Button pauseModeBackToGameButton;

    [Header("Mode Scripts")]
    [SerializeField] private GameModeUIBase[] modes;

    [Header("Mode")]
    [SerializeField] private LocalizeStringEvent[] currentModeNameLocales;
    [SerializeField] private LocalizeStringEvent currentModeInfoLocale;
    [SerializeField] private Transform modeCardParent;
    [SerializeField] private ChooseGameModeCard modeCardPrefab;

    [Header("Phase")]
    [SerializeField] private Transform phaseModeParent;
    [SerializeField] private CanvasGroupController phaseModePrefab;
    [SerializeField] private ChooseGamePhaseCard phasePrefab;

    [Header("References for Game Mode UI")]
    public LocalizeStringEvent GameTurnLocale;
    public LocalizeStringEvent GameOverLocale;
    public CanvasGroupController GameTurnParent;
    public CanvasGroupController AdvantageParent;
    public CanvasGroupController PlacementParent;
    public CanvasGroupController PieceParent;
    public CanvasGroupController TileParent;
    public CanvasGroupController KingStateParent;
    public CanvasGroupController PromotionParent;
    public Button FinishBtn;
    public LocalizeStringEvent FinishBtnLocale;

    public Dictionary<int, CanvasGroupController> PhasesModesParent { get; private set; } = new();
    private List<ChooseGamePhaseCard> phases = new();

    private Dictionary<GameModes, GameModeUIBase> gameModesDic = new();
    public GameModeUIBase CurrentGameMode { get; private set; }

    public override InitPriority Priority => InitPriority.ChooseGameModeUI;

    public override void OnInitAwake()
    {
        SetScreens();

        foreach(var mode in modes)
        {
            gameModesDic.Add(mode.GameModeSO.modeName, mode);

            var modeSO = mode.GameModeSO;

            var modeCard = Instantiate(modeCardPrefab, modeCardParent);
            var phaseMode = Instantiate(phaseModePrefab, phaseModeParent);
            PhasesModesParent.Add((int)modeSO.modeName, phaseMode);

            modeCard.Init(modeSO, false);

            Transform modePhases = phaseMode.GetComponentInChildren<GridLayoutGroup>().transform;

            foreach (var phase in modeSO.phases)
            {
                var phaseO = Instantiate(phasePrefab, modePhases);
                phaseO.Init(modeSO, phase);
                phases.Add(phaseO);
            }
        }

        SetGamePaused(false);
    }

    public override void OnInitStart()
    {
        
    }

    public void SetChoosedGameModeName(GameModeSO gameModeSO)
    {
        foreach (var current in currentModeNameLocales)
        {
            current.StringReference = gameModeSO.modeTranslated.modeNameL;
        }
    }

    public void SetCurrentGameMode(GameModes gameMode)
    {
        foreach (var mode in gameModesDic)
        {
            if (mode.Key == gameMode)
            {
                CurrentGameMode = mode.Value;
                mode.Value.enabled = true;

                currentModeInfoLocale.StringReference = mode.Value.GameModeSO.modeTranslated.modeInfoL;

                currentModeInfoLocale.RefreshString();
                LayoutRebuilder.ForceRebuildLayoutImmediate(currentModeInfoLocale.gameObject.GetComponentInParent<RectTransform>());
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

    private void SetGamePaused(bool paused)
    {
        if (BoardController.Instance != null)
        {
            BoardController.Instance.IsGamePaused = paused;
        }
        
        pauseModeBackToGameButton.gameObject.SetActive(paused);
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

        if (screen == GameScreen.Pause)
        {
            SetGamePaused(true);
        }
        else if (screen == GameScreen.Finish || screen == GameScreen.Play)
        {
            SetGamePaused(false);
        }
    }

    public void ShowScreenNoHideOthers(GameScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            if (view.GameScreen == screen)
            {
                view.SetActive(true);
            }
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

    public void SetInteratableScreen(GameScreen screen, bool interactable)
    {
        foreach (var view in _screenMap.Values)
        {
            if (view.GameScreen == screen)
            {
                view.SetInteractable(interactable);
            }
        }
    }

    public void ShowGameModeInfoScreen(bool show)
    {
        if (show)
        {
            ShowScreenNoHideOthers(GameScreen.GameModeInfo);
            SetInteratableScreen(GameScreen.Play, false);
        }
        else
        {
            SetInteratableScreen(GameScreen.Play, true);
            HideScreen(GameScreen.GameModeInfo);
        }
    }
    #endregion
}
