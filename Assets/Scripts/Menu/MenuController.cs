using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : UnityMethodsSingleton<MenuController>
{
    [Header("Change Screen")]
    [SerializeField] private List<CanvasGroupController> screens;
    private Dictionary<MenuScreen, CanvasGroupController> _screenMap;

    [Header("Tutorial")]
    [SerializeField] private CanvasGroupController modeCardParent;
    [SerializeField] private ChooseGameModeCard modeCardPrefab;
    [SerializeField] private TextMeshProUGUI currentModeNameText;
    [SerializeField] private TextMeshProUGUI currentTutorialDescText;
    [SerializeField] private Image currentTutorialImage;
    [SerializeField] private Button tutorialBackModesBtn;
    [SerializeField] private Button tutorialNextBtn;
    [SerializeField] private Button tutorialPreviousBtn;
    [SerializeField] private CanvasGroupController tutorialPagesParent;
    private GameModeSO currentTutorialGameMode;
    private int currentTutorialPage;

    public override InitPriority Priority => InitPriority.GameModeUI;

    public override void OnInitAwake()
    {
        SetScreens();
    }

    public override void OnInitStart()
    {

    }

    private void SetScreens()
    {
        _screenMap = new Dictionary<MenuScreen, CanvasGroupController>();

        foreach (var screen in screens)
        {
            _screenMap.Add(screen.MenuScreen, screen);
            screen.SetActive(screen.MenuScreen == MenuScreen.Init);
        }

        SetTutorialModesButtons();
    }

    public void ShowScreen(MenuScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            view.SetActive(view.MenuScreen == screen);
        }
    }

    #region Tutorial
    private void SetTutorialModesButtons()
    {
        foreach(var mode in ChooseGameMode.Instance.GameModesSO)
        {
            var modeCard = Instantiate(modeCardPrefab, modeCardParent.transform);
            modeCard.Init(mode, true);
        }

        tutorialBackModesBtn.onClick.AddListener(BackToTutorialModesScreen);
    }

    private void BackToTutorialModesScreen()
    {
        currentModeNameText.text = "Tutorial";
        tutorialBackModesBtn.gameObject.SetActive(false);
        tutorialNextBtn.gameObject.SetActive(false);
        tutorialPreviousBtn.gameObject.SetActive(false);
        tutorialPagesParent.SetActive(false);
        modeCardParent.SetActive(true);
    }

    public void SetTutorialScreens(GameModeSO gameMode)
    {
        tutorialPagesParent.SetActive(true);
        modeCardParent.SetActive(false);
        tutorialBackModesBtn.gameObject.SetActive(true);
        currentTutorialGameMode = gameMode;

        var modeT = gameMode.modeTranslated.modeTutorial;

        if (modeT.Length > 1)
        {
            currentTutorialPage = 0;
            tutorialNextBtn.gameObject.SetActive(true);
            tutorialPreviousBtn.gameObject.SetActive(true);
            tutorialNextBtn.interactable = true;
            tutorialPreviousBtn.interactable = false;
            tutorialNextBtn.onClick.AddListener(() => ChangeTutorialPage(true));
            tutorialPreviousBtn.onClick.AddListener(() => ChangeTutorialPage(false));
        }
        else
        {
            tutorialNextBtn.gameObject.SetActive(false);
            tutorialPreviousBtn.gameObject.SetActive(false);
        }
        
        currentModeNameText.text = gameMode.modeTranslated.modeNameL.GetLocalizedString();
        currentTutorialImage.sprite = modeT[0].tutorialSprite;
        currentTutorialDescText.text = modeT[0].tutorialDescriptionL.GetLocalizedString();
    }

    private void ChangeTutorialPage(bool next)
    {
        var modeT = currentTutorialGameMode.modeTranslated.modeTutorial;

        if (next)
        {
            currentTutorialPage++;
            tutorialPreviousBtn.interactable = true;
            if (modeT.Length <= currentTutorialPage + 1)
            {
                tutorialNextBtn.interactable = false;
            }
        }
        else
        {
            currentTutorialPage--;
            tutorialNextBtn.interactable = true;
            if (currentTutorialPage <= 0)
            {
                tutorialPreviousBtn.interactable = false;
            }
        }

        currentTutorialImage.sprite = modeT[currentTutorialPage].tutorialSprite;
        currentTutorialDescText.text = modeT[currentTutorialPage].tutorialDescriptionL.GetLocalizedString();
    }
    #endregion
}
