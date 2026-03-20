using System.Collections.Generic;
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

        var phase = PhaseController.Instance.CurrentPhase;
        var defs = new List<PieceDefinitionSO>(phase.availablePiecesPromotion);
        defs.AddRange(phase.availablePiecesAI);

        var rng = new System.Random();
        for (int i = defs.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (defs[i], defs[j]) = (defs[j], defs[i]);
        }

        var def = phase.availablePiecesAI[0];
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
