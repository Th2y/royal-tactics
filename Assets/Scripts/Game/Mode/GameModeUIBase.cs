using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private string timeToPlayerPlay = "É a sua vez de jogar";
    [SerializeField] private string timeToAIPlay = "É a vez do oponente jogar";
    [SerializeField] private string timeToPlayerPlacement = "É a sua vez de colocar as peças";
    [SerializeField] private string timeToAIPlacement = "É a vez do oponente colocar as peças";
    [SerializeField] private string playerWin = "Você venceu!";
    [SerializeField] private string playerLost = "Você perdeu!";
    private TextMeshProUGUI gameTurnText;
    private TextMeshProUGUI gameOverText;

    public void SetGameTurnText(GameTurn newTurn)
    {
        switch (newTurn)
        {
            case GameTurn.OpponentPlacement:
                gameTurnText.text = timeToAIPlacement;
                break;
            case GameTurn.PlayerPlacement:
                gameTurnText.text = timeToPlayerPlacement;
                break;
            case GameTurn.OpponentTurn:
                gameTurnText.text = timeToAIPlay;
                break;
            case GameTurn.PlayerTurn:
                gameTurnText.text = timeToPlayerPlay;
                break;
            case GameTurn.GameOverWin:
                gameOverText.text = playerWin;
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Finish);
                break;
            case GameTurn.GameOverLost:
                gameOverText.text = playerLost;
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Finish);
                break;
        }
    }
    #endregion  

    [Header("Play Parts")]
    [SerializeField] private string finishBtnMessage = "Finalizar";
    protected CanvasGroupController gameTurnParent;
    protected CanvasGroupController advantageParent;
    protected CanvasGroupController placementParent;
    protected CanvasGroupController pieceParent;
    protected CanvasGroupController tileParent;
    protected CanvasGroupController promotionParent;
    protected Button finishBtn;
    private TextMeshProUGUI finishBtnTxt;

    protected virtual void SetPlayParts()
    {
        var choose = ChooseGameModeUI.Instance;

        gameTurnText = choose.GameTurnText;
        gameOverText = choose.GameOverText;

        gameTurnParent = choose.GameTurnParent;
        advantageParent = choose.AdvantageParent;
        placementParent = choose.PlacementParent;
        pieceParent = choose.PieceParent;
        tileParent = choose.TileParent;
        promotionParent = choose.PromotionParent;
        finishBtn = choose.FinishBtn;
        finishBtnTxt = choose.FinishBtnTxt;

        finishBtnTxt.text = finishBtnMessage;
    }

    public abstract void BuildUI();
    public abstract void RefreshButtons();

    public abstract void SetOptions(List<PieceDefinitionSO> options);

    public abstract void SetOptions(List<TileName> options);
}
