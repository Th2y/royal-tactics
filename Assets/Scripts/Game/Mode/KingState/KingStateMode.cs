using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingStateMode : GameModeBase
{
    private King playerKing;
    private KingState kingState;

    public List<KingStateDefinitionSO> KingStateDefinitions;

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
                PuzzleTemplateSO template = BoardController.Instance.GeneratePhase(PhaseController.Instance.CurrentPhase);
                SpawnTemplate(template);
                kingState = playerKing.EvaluateKingState();
                break;
            case GameTurn.GameOverWin:
                PhaseController.Instance.ShowNextPhaseButton(true);
                break;
            case GameTurn.GameOverLost:
                PhaseController.Instance.ShowNextPhaseButton(false);
                break;
        }

        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(KingStateDefinitions);
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

    private void SpawnTemplate(PuzzleTemplateSO template)
    {
        bool hasPawn = template.pieces.Any(p => p.pieceOptions.Any(def => def.type == PieceType.Pawn));
        int transform = hasPawn ? Random.Range(0, 2) : Random.Range(0, 8);

        bool spawnInversedColors = Random.Range(0, 2) == 1;

        foreach (var p in template.pieces)
        {
            if (Random.Range(0, 100) > p.spawnChance)
                continue;

            PieceDefinitionSO piece = p.pieceOptions[Random.Range(0, p.pieceOptions.Count)];

            Tile tile;

            if (p.constraint == PositionConstraint.Fixed)
            {
                int index = Random.Range(0, p.positions.Count);
                Vector2Int pos = BoardController.Instance.Transform(p.positions[index], transform);
                tile = BoardController.Instance.GetTile(pos.x, pos.y);
            }
            else
            {
                tile = BoardController.Instance.FindTileForConstraint(p.constraint, playerKing);
            }

            if (tile == null || tile.IsOccupied) continue;

            PlacePiece(tile, piece, p.isPlayer, spawnInversedColors);
        }
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool isFromPlayer, bool isInversed)
    {
        Piece piece = Instantiate(def.prefab);

        piece.Initialize(def, isFromPlayer ^ isInversed);

        tile.SetPiece(piece);

        if (piece.IsFromPlayer ^ isInversed && piece is King king)
        {
            playerKing = king;
            king.CurrentTile.SetOccupiedMarker(true);
        }

        return piece;
    }
}
