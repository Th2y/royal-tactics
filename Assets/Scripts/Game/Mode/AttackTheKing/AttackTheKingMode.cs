using System.Linq;
using UnityEngine;

public class AttackTheKingMode : GameModeBase
{
    private King aiKing;

    public override void StartMode()
    {
        base.StartMode();
        SetGameTurn(GameTurn.OpponentPlacement);

        HumanPlayerUI.Instance.PlayerDoAnything += PlayerFinishedMoves;
    }

    private void OnDisable()
    {
        HumanPlayerUI.Instance.PlayerDoAnything -= PlayerFinishedMoves;
    }

    public override void SetGameTurn(GameTurn newPhase)
    {
        if(newPhase == GameTurn.OpponentTurn)
        {
            SetGameTurn(CheckPlayerWin() ? GameTurn.GameOverWin : GameTurn.GameOverLost);
        }

        CurrentPhase = newPhase;
        ChooseGameModeUI.Instance.CurrentGameMode.SetGameTurnText(newPhase);

        switch (newPhase)
        {
            case GameTurn.OpponentPlacement:
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Play);
                PuzzleTemplateSO template = BoardController.Instance.GeneratePhase(PhaseController.Instance.CurrentPhase, KingState.Stalemate);
                SpawnTemplate(template);
                ChooseGameMode.Instance.CurrentGameMode.SetGameTurn(GameTurn.PlayerPlacement);
                break;
            case GameTurn.GameOverWin:
                PhaseController.Instance.ShowNextPhaseButton(true);
                break;
            case GameTurn.GameOverLost:
                PhaseController.Instance.ShowNextPhaseButton(false);
                break;
        }

        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(PhaseController.Instance.CurrentPhase.availablePiecesHuman);;
    }

    public override void PlayerFinishedMoves()
    {
        SetGameTurn(GameTurn.OpponentTurn);
    }

    protected override bool CheckAIWin()
    {
        return false;
    }

    protected override bool CheckPlayerWin()
    {
        KingState kingState = aiKing.EvaluateKingState();

        if (kingState == KingState.Checkmate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpawnTemplate(PuzzleTemplateSO template)
    {
        bool hasPawn = template.pieces.Any(p => p.pieceOptions.Any(def => def.type == PieceType.Pawn));
        int transform = hasPawn ? Random.Range(0, 2) : Random.Range(0, 8);

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
                tile = BoardController.Instance.FindTileForConstraint(p.constraint, aiKing);
            }

            if (tile == null || tile.IsOccupied) continue;

            PlacePiece(tile, piece, !p.isPlayer);
        }
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool isFromPlayer)
    {
        Piece piece = Instantiate(def.prefab);

        piece.Initialize(def, isFromPlayer);

        tile.SetPiece(piece);

        if (!piece.IsFromPlayer && piece is King king)
        {
            aiKing = king;
            king.CurrentTile.SetOccupiedMarker(true);
        }

        return piece;
    }
}
