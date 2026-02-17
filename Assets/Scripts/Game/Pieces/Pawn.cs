using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private int forwardDirection => isFromPlayer ? 1 : -1;

    public bool CanPromote
    {
        get
        {
            int y = (int)currentTile.pos.y;

            return isFromPlayer
                ? y == 7
                : y == 0;
        }
    }

    public override List<Tile> GetValidMoves(BoardController board)
    {
        List<Tile> moves = new();

        int x = (int)currentTile.pos.x;
        int y = (int)currentTile.pos.y;

        int forwardY = y + forwardDirection;

        Tile forwardTile = board.GetTile(x, forwardY);

        if (forwardTile != null && !forwardTile.IsOccupied)
        {
            moves.Add(forwardTile);
        }

        return moves;
    }

    public override List<Tile> GetValidCaptures(BoardController board)
    {
        List<Tile> captures = new();

        int x = (int)currentTile.pos.x;
        int y = (int)currentTile.pos.y;

        int targetY = y + forwardDirection;

        TryAddCapture(board, x - 1, targetY, captures);
        TryAddCapture(board, x + 1, targetY, captures);

        return captures;
    }

    private void TryAddCapture(BoardController board, int x, int y, List<Tile> captures)
    {
        Tile tile = board.GetTile(x, y);

        if (tile == null || !tile.IsOccupied) return;

        if (tile.Piece.isFromPlayer != isFromPlayer)
        {
            captures.Add(tile);
        }
    }
}