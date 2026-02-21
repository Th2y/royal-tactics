public class PointAdvantageMode : GameModeBase
{
    public override void StartMode()
    {
        base.StartMode();
        SetGameTurn(GameTurn.OpponentPlacement);
    }

    public override void SetGameTurn(GameTurn newPhase)
    {
        if (newPhase == GameTurn.PlayerTurn || newPhase == GameTurn.OpponentTurn)
        {
            bool playerWin = CheckPlayerWin();
            if (playerWin)
            {
                PhaseController.Instance.ShowNextPhaseButton(true);
                SetGameTurn(GameTurn.GameOverWin);
                return;
            }

            bool aiWin = CheckAIWin();
            if (aiWin)
            {
                PhaseController.Instance.ShowNextPhaseButton(false);
                SetGameTurn(GameTurn.GameOverLost);
                return;
            }
        }

        CurrentPhase = newPhase;

        ChooseGameModeUI.Instance.CurrentGameMode.SetGameTurnText(newPhase);

        switch (newPhase)
        {
            case GameTurn.OpponentPlacement:
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Play);
                ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();
                AIController.Instance.StartPlacement();
                break;
            case GameTurn.OpponentTurn:
                ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();
                AIController.Instance.Play();
                break;
            case GameTurn.PlayerTurn:
                HumanPlayerController.Instance.CanMove = true;
                ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();
                break;
            default:
                ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();
                break;
        }
    }

    public override void PlayerFinishedMoves()
    {
        if (CurrentPhase == GameTurn.PlayerPlacement)
        {
            RevealAIPieces();
        }

        SetGameTurn(GameTurn.OpponentTurn);
    }

    protected override bool CheckPlayerWin()
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

    protected override bool CheckAIWin()
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
