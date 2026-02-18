using UnityEngine;

public enum GamePhase
{
    OpponentPlacement,
    PlayerPlacement,
    OpponentTurn,
    PlayerTurn,
    GameOverWin,
    GameOverLost
}

public class GameStateController : UnityMethodsSingleton<GameStateController>
{
    public GamePhase CurrentPhase { get; private set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GamePhase.PlayerTurn ||
        CurrentPhase == GamePhase.PlayerPlacement;

    public override InitPriority Priority => InitPriority.GameStateController;

    public override void OnInitAwake()
    {

    }

    public override void OnInitStart()
    {
        SetPhase(GamePhase.OpponentPlacement);
    }

    public void SetPhase(GamePhase newPhase)
    {
        if (newPhase == GamePhase.PlayerTurn || newPhase == GamePhase.OpponentTurn)
        {
            bool playerWin = CheckPlayerWin();
            if (playerWin)
            {
                PhaseController.Instance.ShowNextPhaseButton(true);
                SetPhase(GamePhase.GameOverWin);
                return;
            }

            bool aiWin = CheckAIWin();
            if (aiWin)
            {
                PhaseController.Instance.ShowNextPhaseButton(false);
                SetPhase(GamePhase.GameOverLost);
                return;
            }
        }

        CurrentPhase = newPhase;

        UIGameController.Instance.SetGamePhaseText(newPhase);

        switch (newPhase)
        {
            case GamePhase.OpponentPlacement:
                UIGameController.Instance.ShowScreen(GameScreen.Play);
                HumanPlayerUI.Instance.RefreshButtons();
                AIController.Instance.StartPlacement();
                break;
            case GamePhase.OpponentTurn:
                HumanPlayerUI.Instance.RefreshButtons();
                AIController.Instance.Play();
                break;
            case GamePhase.PlayerTurn:
                HumanPlayerController.Instance.CanMove = true;
                HumanPlayerUI.Instance.RefreshButtons();
                break;
            default:
                HumanPlayerUI.Instance.RefreshButtons();
                break;
        }
    }

    public void PlayerFinishedMoves()
    {
        if(CurrentPhase  == GamePhase.PlayerPlacement)
        {
            RevealAIPieces();
        }

        SetPhase(GamePhase.OpponentTurn);
    }

    private void RevealAIPieces()
    {
        foreach (var piece in AIController.Instance.PlacedPieces)
        {
            piece.SetVisible(true);
            piece.currentTile.SetOccupiedMarker(false);
        }
    }

    private bool CheckPlayerWin()
    {
        if (HumanPlayerController.Instance.TotalCoins >= AIController.Instance.TotalCoins + PhaseController.Instance.CurrentPhase.pointsAdvantageToWin)
        {
            return true;
        }
        else if (!AIController.Instance.CheckCanDoAnything())
        {
            return true;
        }

        return false;
    }

    private bool CheckAIWin()
    {
        if (AIController.Instance.TotalCoins >= HumanPlayerController.Instance.TotalCoins + PhaseController.Instance.CurrentPhase.pointsAdvantageToWin)
        {
            return true;
        }
        else if (!HumanPlayerController.Instance.CheckCanDoAnything())
        {
            return true;
        }

        return false;
    }
}
