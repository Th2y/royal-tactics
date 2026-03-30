using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public abstract class GameModeUIBase : MonoBehaviour
{
    public GameModeSO GameModeSO;

    protected virtual void OnEnable()
    {
        SetPlayParts();
    }

    protected virtual void OnDestroy()
    {
        
    }

    public virtual void SetInitialValues()
    {

    }

    #region Phrase by Game Turn
    [Header("Game Turn")]
    [SerializeField] private LocalizedString timeToPlayerPlay;
    [SerializeField] private LocalizedString timeToOpponentPlay;
    [SerializeField] private LocalizedString timeToPlayerPlacement;
    [SerializeField] private LocalizedString timeToOpponentPlacement;
    [SerializeField] private LocalizedString playerWin;
    [SerializeField] private LocalizedString playerLost;
    private LocalizeStringEvent gameTurnText;
    private LocalizeStringEvent gameOverText;

    public void SetGameTurnText(GameTurn newTurn)
    {
        switch (newTurn)
        {
            case GameTurn.OpponentPlacement:
                gameTurnText.StringReference = timeToOpponentPlacement;
                break;
            case GameTurn.PlayerPlacement:
                gameTurnText.StringReference = timeToPlayerPlacement;
                break;
            case GameTurn.OpponentTurn:
                gameTurnText.StringReference = timeToOpponentPlay;
                break;
            case GameTurn.PlayerTurn:
                gameTurnText.StringReference = timeToPlayerPlay;
                break;
            case GameTurn.GameOverWin:
                gameOverText.StringReference = playerWin;
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Finish);
                break;
            case GameTurn.GameOverLost:
                gameOverText.StringReference = playerLost;
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Finish);
                break;
        }
    }
    #endregion  

    [Header("Play Parts")]
    [SerializeField] private LocalizedString finishBtnMessage;
    protected CanvasGroupController gameTurnParent;
    protected CanvasGroupController advantageParent;
    protected CanvasGroupController placementParent;
    protected CanvasGroupController pieceParent;
    protected CanvasGroupController tileParent;
    protected CanvasGroupController kingStateParent;
    protected CanvasGroupController promotionParent;
    protected Button finishBtn;
    private LocalizeStringEvent finishBtnTxt;

    protected virtual void SetPlayParts()
    {
        var choose = ChooseGameModeUI.Instance;

        gameTurnText = choose.GameTurnLocale;
        gameOverText = choose.GameOverLocale;

        gameTurnParent = choose.GameTurnParent;
        advantageParent = choose.AdvantageParent;
        placementParent = choose.PlacementParent;
        pieceParent = choose.PieceParent;
        tileParent = choose.TileParent;
        kingStateParent = choose.KingStateParent;
        promotionParent = choose.PromotionParent;
        finishBtn = choose.FinishBtn;
        finishBtnTxt = choose.FinishBtnLocale;

        finishBtnTxt.StringReference = finishBtnMessage;
    }

    public abstract void BuildUI();
    public abstract void RefreshButtons();

    public abstract void SetOptions<T>(List<T> options);
}
