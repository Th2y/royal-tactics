using System.Collections.Generic;
using UnityEngine;

public class BoardController : UnityMethodsSingleton<BoardController>
{
    private Tile[,] tiles = new Tile[8, 8];

    public override InitPriority Priority => InitPriority.BoardController;

    public override void OnInitAwake()
    {
        CacheTiles();
    }

    public override void OnInitStart()
    {

    }

    private void CacheTiles()
    {
        Tile[] children = GetComponentsInChildren<Tile>();

        foreach (Tile tile in children)
        {
            tiles[(int)tile.Position.x, (int)tile.Position.y] = tile;
        }
    }

    public void HandleColliderAllTiles(bool enabled)
    {
        foreach (Tile tile in tiles)
        {
            tile.HandleCollider(enabled);
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (tiles[x, y] == null) CacheTiles();

        if (x < 0 || y < 0 || x >= tiles.GetLength(0) || y >= tiles.GetLength(1))
        {
            Debug.LogWarning($"GetTile fora do limite: x={x}, y={y}");
            return null;
        }

        return tiles[x, y];
    }

    public Tile GetTile(TileName tileName)
    {
        string name = tileName.ToString();

        char fileChar = name[0];
        char rankChar = name[1];

        int x = fileChar - 'A';
        int y = rankChar - '1';

        return GetTile(x, y);
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

    public List<Piece> GetAllPieces()
    {
        List<Piece> pieces = new();

        foreach(Tile tile in tiles)
        {
            if(tile.IsOccupied)
                pieces.Add(tile.Piece);
        }

        return pieces;
    }

    public List<Piece> GetAllPlayerPieces(bool isFromPlayer)
    {
        List<Piece> pieces = new();

        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && tile.Piece.IsFromPlayer == isFromPlayer)
                pieces.Add(tile.Piece);
        }

        return pieces;
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

    public void ClearBoard()
    {
        foreach (Tile tile in tiles)
        {
            tile.ClearAndDestroyPiece();
        }
    }

    public void ClearBoardPlayer(bool isFromPlayer)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && tile.Piece.IsFromPlayer == isFromPlayer) tile.ClearAndDestroyPiece();
        }
    }

    public Tile ChooseTileToInstantiateNewPiece(List<Tile> freeTiles, PieceDefinitionSO piece, bool isHuman)
    {
        var validTiles = piece.prefab.FilterValidSpawnTiles(freeTiles, isHuman);

        if (validTiles == null || validTiles.Count == 0) return null;

        return validTiles[Random.Range(0, validTiles.Count)];
    }

    public List<Tile> GetTilesAttackingTile(Tile targetTile, PieceDefinitionSO pieceDef, bool isFromPlayer)
    {
        List<Tile> result = new();

        List<Tile> freeTiles = GetAllFreeTiles();

        foreach (Tile tile in freeTiles)
        {
            if (WouldPieceAttackTile(tile, targetTile, pieceDef, isFromPlayer))
                result.Add(tile);
        }

        return result;
    }

    private bool WouldPieceAttackTile(Tile originTile, Tile targetTile, PieceDefinitionSO pieceDef, bool isFromPlayer)
    {
        Piece piece = Instantiate(pieceDef.prefab);
        piece.Initialize(pieceDef, isFromPlayer);

        originTile.SetPiece(piece);

        List<Tile> captures = piece.GetValidCaptures(Instance);

        bool attacks = captures.Contains(targetTile);

        originTile.ClearAndDestroyPiece();

        return attacks;
    }

    public List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> result = new();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                Tile t = GetTile((int)tile.Position.x + dx, (int)tile.Position.y + dy);

                if (t != null)
                    result.Add(t);
            }
        }

        return result;
    }
}
