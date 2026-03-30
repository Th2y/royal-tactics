using UnityEngine;

public class HouseNamesMode : GameModeBase
{
    private TileName correctType;

    public override void StartMode()
    {
        base.StartMode();
        SetGameTurn(GameTurn.OpponentPlacement);
    }

    public override void SetGameTurn(GameTurn newPhase)
    {
        switch (newPhase)
        {
            case GameTurn.OpponentPlacement:
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Play);
                SetTile();
                break;
            case GameTurn.GameOverWin:
                PhaseController.Instance.ShowNextPhaseButton(true);
                break;
            case GameTurn.GameOverLost:
                PhaseController.Instance.ShowNextPhaseButton(false);
                break;
        }

        ChooseGameModeUI.Instance.CurrentGameMode.SetGameTurnText(newPhase);
    }

    public override void PlayerFinishedMoves()
    {

    }

    protected override bool CheckAIWin()
    {
        return false;
    }

    protected override bool CheckPlayerWin()
    {
        return false;
    }

    private void SetTile()
    {
        BoardController.Instance.ClearBoard();

        var defs = PhaseController.Instance.CurrentPhase.tilesNames;
        correctType = defs[Random.Range(0, defs.Count)];

        Tile tile = BoardController.Instance.GetTile(correctType);
        tile.SetOccupiedMarker(true);

        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(defs);
    }

    public void OnPlayerGuess(TileName guessedType)
    {
        if (guessedType == correctType)
        {
            SetGameTurn(GameTurn.GameOverWin);
        }
        else
        {
            SetGameTurn(GameTurn.GameOverLost);
        }
    }
}
