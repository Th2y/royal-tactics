using UnityEngine;

public class MysteryPieceMode : GameModeBase
{
    private Piece mysteryPiece;
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
                SetPiece();
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

    protected override bool CheckPlayerWin()
    {
        return false;
    }

    protected override bool CheckAIWin()
    {
        return false;
    }

    private void SetPiece()
    {
        BoardController.Instance.ClearBoard();

        var defs = PhaseController.Instance.CurrentPhase.availablePiecesAI;
        var def = defs[Random.Range(0, defs.Count)];

        correctType = def.type;

        Tile tile = BoardController.Instance.ChooseTileToInstantiateNewPiece(BoardController.Instance.GetAllFreeTiles(), def, true);
        tile.SetOccupiedMarker(true);

        mysteryPiece = Instantiate(def.prefab);
        mysteryPiece.Initialize(def, true);
        mysteryPiece.SetVisible(false);
        tile.SetPiece(mysteryPiece);

        HumanPlayerController.Instance.HighlightValidTiles(mysteryPiece);
        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(defs);
    }

    public void OnPlayerGuess(PieceType guessedType)
    {
        if(guessedType == correctType)
        {
            SetGameTurn(GameTurn.GameOverWin);
        }
        else
        {
            SetGameTurn(GameTurn.GameOverLost);
        }
    }
}
