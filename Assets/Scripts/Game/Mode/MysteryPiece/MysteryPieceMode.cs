using System.Collections.Generic;
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
        var board = BoardController.Instance;

        board.ClearBoard();
        List<Tile> freeTiles = board.GetAllFreeTiles();

        var defsAI = PhaseController.Instance.CurrentPhase.availablePiecesAI;
        var defsHuman = PhaseController.Instance.CurrentPhase.availablePiecesHuman;
        var def = defsAI[0];

        correctType = def.type;

        Tile tile = board.ChooseTileToInstantiateNewPiece(def, true, freeTiles);
        tile.SetOccupiedMarker(true);
        freeTiles.Remove(tile);

        mysteryPiece = Instantiate(def.prefab);
        mysteryPiece.Initialize(def, true);
        mysteryPiece.SetVisible(false);
        tile.SetPiece(mysteryPiece);

        for (int i = 0; i < defsHuman.Count; i++)
        {
            Tile tileP = board.ChooseTileToInstantiateNewPiece(def, true, freeTiles);
            var piece = Instantiate(defsHuman[i].prefab);
            piece.Initialize(def, false);
            tileP.SetPiece(piece);
            freeTiles.Remove(tileP);
        }

        HumanPlayerController.Instance.HighlightValidTiles(mysteryPiece);
        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(defsAI);
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
