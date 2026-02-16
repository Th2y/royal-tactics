using UnityEngine;
using System.Collections.Generic;

public class Bishop : Piece
{
    private static readonly Vector2Int[] directions =
    {
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1)
    };

    public override List<Tile> GetValidMoves(BoardController board)
    {
        List<Tile> moves = new();

        int startX = (int)currentTile.pos.x;
        int startY = (int)currentTile.pos.y;

        foreach (var dir in directions)
        {
            int x = startX + dir.x;
            int y = startY + dir.y;

            while (true)
            {
                Tile tile = board.GetTile(x, y);

                if (tile == null)
                    break;

                if (tile.IsOccupied)
                    break;

                moves.Add(tile);

                x += dir.x;
                y += dir.y;
            }
        }

        return moves;
    }

    public override List<Tile> GetValidCaptures(BoardController board)
    {
        List<Tile> captures = new();

        int startX = (int)currentTile.pos.x;
        int startY = (int)currentTile.pos.y;

        foreach (var dir in directions)
        {
            int x = startX + dir.x;
            int y = startY + dir.y;

            while (true)
            {
                Tile tile = board.GetTile(x, y);

                if (tile == null)
                    break;

                if (!tile.IsOccupied)
                {
                    x += dir.x;
                    y += dir.y;
                    continue;
                }

                if (tile.piece.isFromPlayer != isFromPlayer)
                {
                    captures.Add(tile);
                }

                break;
            }
        }

        return captures;
    }
}
