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

    #region Helpers
    private IEnumerable<Tile> GetAdjacentTiles(BoardController board)
    {
        int startX = CurrentTile.Position.x;
        int startY = CurrentTile.Position.y;

        foreach (var dir in directions)
        {
            int x = startX + dir.x;
            int y = startY + dir.y;

            Tile tile = board.GetTile(x, y);

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

        return dx <= 1 && dy <= 1;
    }
    #endregion
}