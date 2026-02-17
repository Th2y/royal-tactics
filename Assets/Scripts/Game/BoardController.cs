using System.Collections.Generic;
using UnityEngine;

public class BoardController : UnityMethods
{
    private Tile[,] tiles = new Tile[8, 8];

    public static BoardController Instance;

    public override InitPriority Priority => InitPriority.BoardController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CacheTiles();
    }

    public override void InitStart()
    {

    }

    private void CacheTiles()
    {
        Tile[] children = GetComponentsInChildren<Tile>();

        foreach (Tile tile in children)
        {
            tiles[(int)tile.pos.x, (int)tile.pos.y] = tile;
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= tiles.GetLength(0) || y >= tiles.GetLength(1))
        {
            Debug.LogWarning($"GetTile fora do limite: x={x}, y={y}");
            return null;
        }

        return tiles[x, y];
    }

    public List<Tile> GetAllFreeTiles()
    {
        List<Tile> free = new();

        foreach (Tile tile in tiles)
        {
            if (!tile.IsOccupied)
                free.Add(tile);
        }

        return free;
    }

    public bool TryPlacePiece(Piece piecePrefab, int x, int y)
    {
        Tile tile = GetTile(x, y);

        if (tile == null || tile.IsOccupied)
            return false;

        Piece piece = Instantiate(piecePrefab);
        tile.SetPiece(piece);

        return true;
    }
}
