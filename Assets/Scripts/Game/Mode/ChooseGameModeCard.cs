using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI modeNameTxt;

    private int modeId;
    private bool isTutorial;
    private GameModeSO gameModeSO;

    public void Init(GameModeSO modeSO, bool isTutorial)
    {
        this.gameModeSO = modeSO;
        this.isTutorial = isTutorial;
        modeId = (int)modeSO.modeName;
        modeNameTxt.text = modeSO.modeTranslated.modeNameL.GetLocalizedString();
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
