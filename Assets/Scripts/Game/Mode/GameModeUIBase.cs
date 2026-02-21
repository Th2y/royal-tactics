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
    [SerializeField] private TextMeshProUGUI gameTurnText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private string timeToPlayerPlay = "É a sua vez de jogar";
    [SerializeField] private string timeToAIPlay = "É a vez do oponente jogar";
    [SerializeField] private string timeToPlayerPlacement = "É a sua vez de colocar as peças";
    [SerializeField] private string timeToAIPlacement = "É a vez do oponente colocar as peças";
    [SerializeField] private string playerWin = "Você venceu!";
    [SerializeField] private string playerLost = "Você perdeu!";

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
    [SerializeField] protected CanvasGroupController gameTurnParent;
    [SerializeField] protected CanvasGroupController advantageParent;
    [SerializeField] protected CanvasGroupController placementParent;
    [SerializeField] protected CanvasGroupController pieceParent;
    [SerializeField] protected CanvasGroupController promotionParent;
    [SerializeField] protected Button finishBtn;
    [SerializeField] private TextMeshProUGUI finishBtnTxt;
    [SerializeField] private string finishBtnMessage = "Finalizar";

    protected virtual void SetPlayParts()
    {
        finishBtnTxt.text = finishBtnMessage;
    }

    public abstract void BuildUI();
    public abstract void RefreshButtons();

    public abstract void SetOptions(List<PieceDefinitionSO> options);
}
