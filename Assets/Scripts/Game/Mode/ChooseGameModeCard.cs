using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ChooseGameModeCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LocalizeStringEvent modeNameLocale;

    private int modeId;
    private bool isTutorial;
    private GameModeSO gameModeSO;

    public void Init(GameModeSO modeSO, bool isTutorial)
    {
        this.gameModeSO = modeSO;
        this.isTutorial = isTutorial;
        modeId = (int)modeSO.modeName;
        modeNameLocale.StringReference = modeSO.modeTranslated.modeNameL;
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isTutorial)
        {
            MenuController.Instance.SetTutorialScreens(gameModeSO);
        }
        else
        {
            foreach (var p in ChooseGameModeUI.Instance.PhasesModesParent)
            {
                p.Value.SetActive(p.Key == modeId);
            }

            ChooseGameModeUI.Instance.ShowScreen(GameScreen.ChooseGamePhase);
        }
    }
}
