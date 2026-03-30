using UnityEngine;
using System.Collections.Generic;

public class Knight : Piece
{
    private static readonly Vector2Int[] directions =
    {
        new(1, 2),
        new(2, 1),
        new(2, -1),
        new(1, -2),
        new(-1, -2),
        new(-2, -1),
        new(-2, 1),
        new(-1, 2)
    };

    public override List<Tile> GetValidMoves(BoardController board)
    {
        List<Tile> moves = new();

        int x = (int)CurrentTile.Position.x;
        int y = (int)CurrentTile.Position.y;

        foreach (var dir in directions)
        {
            Tile target = board.GetTile(x + dir.x, y + dir.y);

            if (target == null)
                continue;

            if (!target.IsOccupied)
            {
                moves.Add(target);
            }
        }

        return moves;
    }

    public override List<Tile> GetValidCaptures(BoardController board, bool fromSamePlayer = false)
    {
        List<Tile> tiles = new();

        int x = (int)CurrentTile.Position.x;
        int y = (int)CurrentTile.Position.y;

        foreach (var dir in directions)
        {
            Tile tile = board.GetTile(x + dir.x, y + dir.y);

            if (tile == null)
                continue;

            if (!tile.IsOccupied)
                continue;

            if (fromSamePlayer)
            {
                if (tile.Piece.IsFromPlayer == IsFromPlayer)
                {
                    tiles.Add(tile);
                }
            }
            else
            {
                if (tile.Piece.IsFromPlayer != IsFromPlayer)
                {
                    tiles.Add(tile);
                }
            }
        }

        return tiles;
    }
}
