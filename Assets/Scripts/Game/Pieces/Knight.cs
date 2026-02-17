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

        int x = (int)currentTile.pos.x;
        int y = (int)currentTile.pos.y;

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

    public override List<Tile> GetValidCaptures(BoardController board)
    {
        List<Tile> captures = new();

        int x = (int)currentTile.pos.x;
        int y = (int)currentTile.pos.y;

        foreach (var dir in directions)
        {
            Tile target = board.GetTile(x + dir.x, y + dir.y);

            if (target == null)
                continue;

            if (!target.IsOccupied)
                continue;

            if (target.Piece.isFromPlayer != isFromPlayer)
            {
                captures.Add(target);
            }
        }

        return captures;
    }
}
