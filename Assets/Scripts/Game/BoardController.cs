using System.Collections.Generic;
using UnityEngine;

public class BoardController : UnityMethodsSingleton<BoardController>
{
    private Tile[,] tiles = new Tile[8, 8];

    public bool IsGamePaused = false;

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
        if (x < 0 || y < 0 || x >= tiles.GetLength(0) || y >= tiles.GetLength(1))
        {
            Debug.LogWarning($"GetTile fora do limite: x={x}, y={y}");
            return null;
        }

        if (tiles[x, y] == null)
        {
            CacheTiles();
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
        List<Tile> tiles = new();

        foreach (Tile tile in this.tiles)
        {
            if (!tile.IsOccupied)
                tiles.Add(tile);
        }

        return tiles;
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

    public Tile ChooseTileToInstantiateNewPiece(PieceDefinitionSO piece, bool isHuman, List<Tile> freeTiles = null)
    {
        if (freeTiles == null || freeTiles.Count == 0) freeTiles = GetAllFreeTiles();

        if (freeTiles.Count == 0)
        {
            Debug.LogWarning("Não existem casas disponíveis");
            return null;
        }

        var validTiles = piece.prefab.FilterValidSpawnTiles(freeTiles, isHuman);

        if (validTiles == null || validTiles.Count == 0) return null;

        return validTiles[Random.Range(0, validTiles.Count)];
    }

    public Tile FindTileForConstraint(PositionConstraint constraint, King playerKing)
    {
        List<Tile> freeTiles = GetAllFreeTiles();

        List<Tile> candidates = new();

        foreach (Tile tile in freeTiles)
        {
            Vector2Int pos = tile.Position;

            switch (constraint)
            {
                case PositionConstraint.Any:
                    candidates.Add(tile);
                    break;

                case PositionConstraint.Center:
                    if (pos.x >= 2 && pos.x <= 5 && pos.y >= 2 && pos.y <= 5)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.Edge:
                    if (pos.x == 0 || pos.x == 7 || pos.y == 0 || pos.y == 7)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.Corner:
                    if ((pos.x == 0 || pos.x == 7) && (pos.y == 0 || pos.y == 7))
                        candidates.Add(tile);
                    break;

                case PositionConstraint.NearPlayerKing:
                    if (playerKing != null &&
                        Vector2Int.Distance(pos, playerKing.CurrentTile.Position) <= 2)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.FarFromPlayerKing:
                    if (playerKing != null &&
                        Vector2Int.Distance(pos, playerKing.CurrentTile.Position) >= 4)
                        candidates.Add(tile);
                    break;
            }
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    public Vector2Int Transform(Vector2Int pos, int type)
    {
        int x = pos.x;
        int y = pos.y;

        return type switch
        {
            0 => new Vector2Int(x, y),
            1 => new Vector2Int(7 - x, y),
            2 => new Vector2Int(x, 7 - y),
            3 => new Vector2Int(7 - x, 7 - y),
            4 => new Vector2Int(y, x),
            5 => new Vector2Int(7 - y, x),
            6 => new Vector2Int(y, 7 - x),
            7 => new Vector2Int(7 - y, 7 - x),
            _ => pos,
        };
    }

    public PuzzleTemplateSO GeneratePhase(PhaseSO phase, KingState desired = KingState.None)
    {
        ClearBoard();

        if (desired == KingState.None) desired = PickDesiredState(phase);

        PuzzleTemplateSO template = phase.GetRandomTemplate(desired);

        if (template == null)
        {
            Debug.LogError($"No template for state {desired} in phase {phase.phase}");
            return null;
        }

        return template;
    }

    private KingState PickDesiredState(PhaseSO phase)
    {
        int roll = Random.Range(0, 100);

        if (roll < phase.safeChance)
            return KingState.Safe;

        roll -= phase.safeChance;

        if (roll < phase.checkChance)
            return KingState.Check;

        roll -= phase.checkChance;

        if (roll < phase.checkmateChance)
            return KingState.Checkmate;

        return KingState.Stalemate;
    }
}
