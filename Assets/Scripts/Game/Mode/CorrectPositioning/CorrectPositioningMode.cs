using UnityEngine;

public class CorrectPositioningMode : GameModeBase
{
    private Piece selectedPiece;
    private PieceType correctType;

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

        var defs = PhaseController.Instance.CurrentPhase.availablePiecesAI;
        var def = defs[Random.Range(0, defs.Count)];

        correctType = def.type;

        Tile tile = BoardController.Instance.GetTile(TileName.D4);
        selectedPiece = Instantiate(def.prefab);
        selectedPiece.Initialize(def, true);
        tile.SetPiece(selectedPiece);

        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(defs);
    }

    public void OnPlayerGuess(PieceType guessedType)
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
