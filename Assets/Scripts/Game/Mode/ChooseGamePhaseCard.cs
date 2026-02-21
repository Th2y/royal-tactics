using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGamePhaseCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI phaseTxt;

    private GameModeSO modeSO;
    private PhaseSO phaseSO;

    public void Init(GameModeSO modeSO, PhaseSO phaseSO)
    {
        this.modeSO = modeSO;
        this.phaseSO = phaseSO;

        phaseTxt.text = phaseSO.phase.ToString();

        CheckUnlocked();
        button.onClick.AddListener(OnClick);
    }

    public void CheckUnlocked()
    {
        button.interactable = phaseSO.phase == 1 || GameServices.Progress.IsPhaseCompleted(modeSO.modeId, phaseSO.phase - 1);
    }

    private void OnClick()
    {
        ChooseGameModeUI.Instance.HideScreen(GameScreen.ChooseGamePhase);
        PhaseController.Instance.StartGameMode(modeSO, phaseSO.phase);
    }
}
