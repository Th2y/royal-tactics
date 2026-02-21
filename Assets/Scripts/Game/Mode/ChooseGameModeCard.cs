using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeCard : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI modeNameTxt;

    private int modeId;

    public void Init(GameModeSO modeSO)
    {
        modeId = modeSO.modeId;
        modeNameTxt.text = modeSO.modeNamePt;
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        foreach(var p in ChooseGameModeUI.Instance.PhasesModesParent)
        {
            p.Value.SetActive(p.Key == modeId);
        }

        ChooseGameModeUI.Instance.ShowScreen(GameScreen.ChooseGamePhase);
    }
}
