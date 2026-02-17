using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Tile[] tiles;

    public void Play()
    {
        Invoke(nameof(PlayTurn), 5f);
    }

    private void PlayTurn()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (TryCapture(aiPieces))
            return;

        if (TryPromote(aiPieces))
            return;

        DoRandomMove(aiPieces);
    }

    private List<Piece> GetAIPieces()
    {
        List<Piece> pieces = new();

        if(tiles == null || tiles.Length <= 0)
        {
            tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        }

        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && !tile.piece.isFromPlayer)
                pieces.Add(tile.piece);
        }

        return pieces;
    }

    private bool TryCapture(List<Piece> pieces)
    {
        List<(Piece piece, Tile target, int value)> captures = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
            {
                Piece targetPiece = tile.piece;
                int value = targetPiece.Definition.cost;

                captures.Add((piece, tile, value));
            }
        }

        if (captures.Count == 0)
            return false;

        int bestValue = captures.Max(c => c.value);

        var bestCaptures = captures
            .Where(c => c.value == bestValue)
            .ToList();

        var chosen = bestCaptures[Random.Range(0, bestCaptures.Count)];

        ExecuteMove(chosen.piece, chosen.target);
        return true;
    }

    private bool TryPromote(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (piece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
                return true;
            }
        }

        return false;
    }

    private void DoRandomMove(List<Piece> pieces)
    {
        List<(Piece piece, Tile tile)> moves = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
            {
                moves.Add((piece, tile));
            }
        }

        if (moves.Count == 0)
            return;

        var chosen = moves[Random.Range(0, moves.Count)];
        ExecuteMove(chosen.piece, chosen.tile);
    }

    private void ExecuteMove(Piece piece, Tile target)
    {
        GameStateController.Instance.IsBusy = true;

        Tile origin = piece.currentTile;

        if (target.IsOccupied) Destroy(target.piece.gameObject);

        origin.Clear();
        target.SetPiece(piece);

        piece.MoveToTile(target, 0.35f, () =>
        {
            if (piece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
            }
            else
            {
                GameStateController.Instance.IsBusy = false;
                GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
            }
        });
    }
}
