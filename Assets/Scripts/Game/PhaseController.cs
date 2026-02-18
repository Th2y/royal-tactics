using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseController : UnityMethods
{
    [SerializeField] private Button playAgainFinishBtn;
    [SerializeField] private Button nextPhaseFinishBtn;

    [SerializeField] private List<PhaseSO> phasesList;

    private Dictionary<int, PhaseSO> phasesById;
    private int currentPhaseId;

    public PhaseSO CurrentPhase => phasesById[currentPhaseId];

    public static PhaseController Instance;

    public override InitPriority Priority => InitPriority.PhaseController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildDictionary();

        currentPhaseId = 1;
        playAgainFinishBtn.onClick.AddListener(() => StartPhase(currentPhaseId));
        nextPhaseFinishBtn.onClick.AddListener(() => NextPhase());
    }

    public override void InitStart()
    {
        StartPhase(currentPhaseId);
    }

    private void OnDestroy()
    {
        playAgainFinishBtn.onClick.RemoveAllListeners();
        nextPhaseFinishBtn.onClick.RemoveAllListeners();
    }

    #region Setup
    private void BuildDictionary()
    {
        phasesById = new Dictionary<int, PhaseSO>();

        foreach (var phase in phasesList)
        {
            if (phasesById.ContainsKey(phase.phase))
            {
                Debug.LogError($"Duplicate Phase ID: {phase.phase}");
                continue;
            }

            phasesById.Add(phase.phase, phase);
        }
    }
    #endregion

    #region Flow
    public void StartPhase(int phaseId)
    {
        if (!phasesById.ContainsKey(phaseId))
        {
            Debug.LogError($"Phase {phaseId} not found");
            return;
        }

        currentPhaseId = phaseId;
        ApplyPhase();
    }

    private void NextPhase()
    {
        int nextId = currentPhaseId + 1;

        if (!phasesById.ContainsKey(nextId))
        {
            OnAllPhasesCompleted();
            return;
        }

        StartPhase(nextId);
    }

    public void RestartPhase()
    {
        ApplyPhase();
    }
    #endregion

    #region Internal
    public void ShowNextPhaseButton(bool show)
    {
        nextPhaseFinishBtn.interactable = show;
    }

    private void ApplyPhase()
    {
        UIGameController.Instance.SetInitialValues();
        BoardController.Instance.ClearBoard();
        PlayerController.Instance.InitCoins();
        AIController.Instance.InitCoins();
        GameStateController.Instance.SetPhase(GamePhase.OpponentPlacement);
    }

    private void OnAllPhasesCompleted()
    {
        Debug.Log("All phases completed");
    }
    #endregion
}
