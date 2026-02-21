using System.Collections.Generic;

public class Pawn : Piece
{
    private int forwardDirection => IsFromPlayer ? 1 : -1;

    public override bool CanPromote
    {
        get
        {
            int y = (int)CurrentTile.Position.y;

            return IsFromPlayer
                ? y == 7
                : y == 0;
        }
    }

    public override List<Tile> FilterValidSpawnTiles(List<Tile> freeTiles, bool isHuman)
    {
        if (isHuman)
        {
            return freeTiles.FindAll(t => t.Position.y >= 1 && t.Position.y <= 4);
        }
        else
        {
            return freeTiles.FindAll(t => t.Position.y >= 3 && t.Position.y <= 6);
        }
    }

    public override bool IsValidPlacement(Tile tile, bool isHuman)
    {
        int y = (int)tile.Position.y;

        if (isHuman)
        {
            return y >= 1 && y <= 4;
        }
        else
        {
            return y >= 3 && y <= 6;
        }
    }

    public override List<Tile> GetValidMoves(BoardController board)
    {
        List<Tile> moves = new();

        int x = (int)CurrentTile.Position.x;
        int y = (int)CurrentTile.Position.y;

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

        int x = (int)CurrentTile.Position.x;
        int y = (int)CurrentTile.Position.y;

        int targetY = y + forwardDirection;

        TryAddCapture(board, x - 1, targetY, captures);
        TryAddCapture(board, x + 1, targetY, captures);

        return captures;
    }

    private void TryAddCapture(BoardController board, int x, int y, List<Tile> captures)
    {
        Tile tile = board.GetTile(x, y);

        if (tile == null || !tile.IsOccupied) return;

        if (tile.Piece.IsFromPlayer != IsFromPlayer)
        {
            captures.Add(tile);
        }
    }
}