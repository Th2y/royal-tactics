using UnityEngine;
using UnityEngine.UI;

public class PhaseController : UnityMethodsSingleton<PhaseController>
{
    [SerializeField] private Button playAgainFinishBtn;
    [SerializeField] private Button nextPhaseFinishBtn;

    public GameModeSO CurrentGameMode { get; private set; }
    public int CurrentPhaseIndex { get; private set; }

    public PhaseSO CurrentPhase => CurrentGameMode.phases[CurrentPhaseIndex];

    public override InitPriority Priority => InitPriority.PhaseController;

    public override void OnInitAwake()
    {
        playAgainFinishBtn.onClick.AddListener(() => RestartPhase());
        nextPhaseFinishBtn.onClick.AddListener(() => NextPhase());
    }

    public override void OnInitStart()
    {

    }

    private void OnDestroy()
    {
        playAgainFinishBtn.onClick.RemoveAllListeners();
        nextPhaseFinishBtn.onClick.RemoveAllListeners();
    }

    #region Flow
    public void StartGameMode(GameModeSO mode, int phase)
    {
        ChooseGameMode.Instance.SetCurrentGameMode(mode.modeName);
        CurrentGameMode = mode;
        CurrentPhaseIndex = phase - 1;
        ChooseGameMode.Instance.StartPhase();
    }

    public void NextPhase()
    {
        nextPhaseFinishBtn.interactable = false;

        CurrentPhaseIndex++;

        if (CurrentPhaseIndex >= CurrentGameMode.phases.Count)
        {
            Debug.LogError("Is the last phase of this game mode");
            return;
        }

        ChooseGameMode.Instance.StartPhase();
    }

    public void RestartPhase()
    {
        ChooseGameMode.Instance.StartPhase();
    }
    #endregion

    #region Internal
    public void ShowNextPhaseButton(bool show)
    {
        if (show)
        {
            GameServices.Progress.CompletePhase(CurrentGameMode.modeId, CurrentPhaseIndex + 1);
            ChooseGameModeUI.Instance.ReloadPhases();
        }

        nextPhaseFinishBtn.interactable = show;
    }

    private void OnAllPhasesCompleted()
    {
        Debug.Log("All phases completed");
    }
    #endregion
}
