using UnityEngine;
using System.Collections.Generic;

public class King : Piece
{
    private static readonly Vector2Int[] directions =
    {
        new( 1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1),

        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    };

    #region Movement
    public override List<Tile> GetValidMoves(BoardController board)
    {
        List<Tile> moves = new();

        foreach (var tile in GetAdjacentTiles(board))
        {
            if (tile.IsOccupied)
                continue;

            if (IsTileUnderAttack(board, tile))
                continue;

            moves.Add(tile);
        }

        return moves;
    }

    public override List<Tile> GetValidCaptures(BoardController board)
    {
        List<Tile> captures = new();

        foreach (var tile in GetAdjacentTiles(board))
        {
            if (!tile.IsOccupied)
                continue;

            if (tile.Piece.IsFromPlayer == IsFromPlayer)
                continue;

            if (IsTileUnderAttack(board, tile))
                continue;

            captures.Add(tile);
        }

        return captures;
    }
    #endregion

    #region King State
    public KingState EvaluateKingState()
    {
        if (IsCheckmate(BoardController.Instance))
            return KingState.Checkmate;

        if (IsStalemate(BoardController.Instance))
            return KingState.Stalemate;

        if (IsInCheck(BoardController.Instance))
            return KingState.Check;

        return KingState.Safe;
    }

    public bool IsInCheck(BoardController board)
    {
        return IsTileUnderAttack(board, CurrentTile);
    }

    public bool IsCheckmate(BoardController board)
    {
        if (!IsInCheck(board))
            return false;

        if (GetValidMoves(board).Count > 0)
            return false;

        if (GetValidCaptures(board).Count > 0)
            return false;

        foreach (var piece in board.GetAllPieces())
        {
            if (piece.IsFromPlayer != IsFromPlayer)
                continue;

            if (piece == this)
                continue;

            if (CanPieceSaveKing(board, piece))
                return false;
        }

        return true;
    }

    public bool IsStalemate(BoardController board)
    {
        if (IsInCheck(board))
            return false;

        foreach (var piece in board.GetAllPieces())
        {
            if (piece.IsFromPlayer != IsFromPlayer)
                continue;

            if (piece.GetValidMoves(board).Count > 0)
                return false;

            if (piece.GetValidCaptures(board).Count > 0)
                return false;
        }

        return true;
    }
    #endregion

    #region Helpers
    private IEnumerable<Tile> GetAdjacentTiles(BoardController board)
    {
        int startX = CurrentTile.Position.x;
        int startY = CurrentTile.Position.y;

        foreach (var dir in directions)
        {
            Tile tile = board.GetTile(startX + dir.x, startY + dir.y);

            if (tile != null)
                yield return tile;
        }
    }

    private bool IsTileUnderAttack(BoardController board, Tile targetTile)
    {
        foreach (var piece in board.GetAllPieces())
        {
            if (piece.IsFromPlayer == IsFromPlayer)
                continue;

            if (piece is King enemyKing)
            {
                if (IsAdjacent(enemyKing.CurrentTile, targetTile))
                    return true;

                continue;
            }

            var attacks = piece.GetValidCaptures(board);

            if (attacks.Contains(targetTile))
                return true;
        }

        return false;
    }

    private bool IsAdjacent(Tile from, Tile to)
    {
        int dx = Mathf.Abs(from.Position.x - to.Position.x);
        int dy = Mathf.Abs(from.Position.y - to.Position.y);

        return (dx <= 1 && dy <= 1) && !(dx == 0 && dy == 0);
    }

    private bool CanPieceSaveKing(BoardController board, Piece piece)
    {
        var moves = piece.GetValidMoves(board);
        var captures = piece.GetValidCaptures(board);

        foreach (var tile in moves)
        {
            if (SimulateMoveAndCheck(board, piece, tile))
                return true;
        }

        foreach (var tile in captures)
        {
            if (SimulateMoveAndCheck(board, piece, tile))
                return true;
        }

        return false;
    }

    private bool SimulateMoveAndCheck(BoardController board, Piece piece, Tile targetTile)
    {
        Tile originalTile = piece.CurrentTile;
        Piece capturedPiece = targetTile.Piece;

        // simulate
        originalTile.Piece = null;
        targetTile.Piece = piece;
        piece.CurrentTile = targetTile;

        bool stillInCheck = IsInCheck(board);

        // rollback
        piece.CurrentTile = originalTile;
        originalTile.Piece = piece;
        targetTile.Piece = capturedPiece;

        return !stillInCheck;
    }
    #endregion
}