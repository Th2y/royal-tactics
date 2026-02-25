using System.Collections.Generic;
using UnityEngine;

public class AttackTheKing : GameModeBase
{
    private King playerKing;
    private KingState kingState;

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
                kingState = GeneratePhase(PhaseController.Instance.CurrentPhase);
                Debug.LogError(kingState);
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

    public void OnPlayerGuess(KingState guessedType)
    {
        if (guessedType == kingState)
        {
            SetGameTurn(GameTurn.GameOverWin);
        }
        else
        {
            SetGameTurn(GameTurn.GameOverLost);
        }
    }

    public KingState GeneratePhase(PhaseSO phase)
    {
        KingState desired = PickDesiredState(phase);

        const int MAX_ATTEMPTS = 10;
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            BoardController.Instance.ClearBoard();

            PlacePiecesRandomly(phase.availablePiecesHuman, true);
            PlacePiecesRandomly(phase.availablePiecesAI, false);

            KingState actual = playerKing.EvaluateKingState();

            if (actual == desired) return actual;
        }

        return playerKing.EvaluateKingState();
    }

    private KingState PickDesiredState(PhaseSO phase)
    {
        int roll = Random.Range(0, 100);

        if (roll < phase.safeChance)
            return KingState.Safe;

        roll -= phase.safeChance;

        if (roll < phase.checkChance)
            return KingState.Check;

        roll -= phase.checkChance;

        if (roll < phase.checkmateChance)
            return KingState.Checkmate;

        return KingState.Stalemate;
    }

    private void PlacePiecesRandomly(List<PieceDefinitionSO> pieces, bool isFromPlayer)
    {
        foreach (var pieceDef in pieces)
        {
            List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();
            Tile tile = BoardController.Instance.ChooseTileToInstantiateNewPiece(freeTiles, pieceDef, isFromPlayer);

            PlacePiece(tile, pieceDef, isFromPlayer);
        }
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool isFromPlayer)
    {
        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, isFromPlayer);
        tile.SetPiece(piece);

        if (isFromPlayer && piece is King king)
        {
            playerKing = king;
            king.CurrentTile.SetOccupiedMarker(true);
        }

        return piece;
    }
}
