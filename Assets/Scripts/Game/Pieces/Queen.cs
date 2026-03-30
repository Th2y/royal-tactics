using UnityEngine;
using System.Collections.Generic;

public class Queen : Piece
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

        int startX = (int)CurrentTile.Position.x;
        int startY = (int)CurrentTile.Position.y;

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

    public override List<Tile> GetValidCaptures(BoardController board, bool fromSamePlayer = false)
    {
        List<Tile> tiles = new();

        int startX = (int)CurrentTile.Position.x;
        int startY = (int)CurrentTile.Position.y;

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

                break;
            }
        }

        return tiles;
    }

    public override List<Tile> GetAttackVisionTiles(BoardController board)
    {
        List<Tile> tiles = new();

        int startX = (int)CurrentTile.Position.x;
        int startY = (int)CurrentTile.Position.y;

        foreach (var dir in directions)
        {
            int x = startX + dir.x;
            int y = startY + dir.y;

            while (true)
            {
                Tile tile = board.GetTile(x, y);

                if (tile == null)
                    break;

                tiles.Add(tile);

                if (tile.IsOccupied)
                {
                    if (tile.Piece.IsFromPlayer == IsFromPlayer)
                    {
                        break;
                    }
                    else if (tile.Piece.Definition.type != PieceType.King)
                    {
                        break;
                    }
                }

                x += dir.x;
                y += dir.y;
            }
        }

        return tiles;
    }
}
