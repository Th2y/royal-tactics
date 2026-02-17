using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private PhaseSO phaseSO;

    private Tile[] tiles;

    public List<Piece> placedPieces { get; private set; } = new();

    private int currentPoints;

    public void Play()
    {
        Invoke(nameof(PlayTurn), 5f);
    }

    private void PlayTurn()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (TryCapture(aiPieces))
            return;

        if (TryPromote(aiPieces))
            return;

        if (currentPoints > 0 && TryPlacementDuringTurn())
            return;

        DoRandomMove(aiPieces);
    }

    private List<Piece> GetAIPieces()
    {
        List<Piece> pieces = new();

        if(tiles == null || tiles.Length <= 0)
        {
            tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        }

        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && !tile.piece.isFromPlayer)
                pieces.Add(tile.piece);
        }

        return pieces;
    }

    private bool TryCapture(List<Piece> pieces)
    {
        List<(Piece piece, Tile target, int value)> captures = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
            {
                Piece targetPiece = tile.piece;
                int value = targetPiece.Definition.cost;

                captures.Add((piece, tile, value));
            }
        }

        if (captures.Count == 0)
            return false;

        int bestValue = captures.Max(c => c.value);

        var bestCaptures = captures
            .Where(c => c.value == bestValue)
            .ToList();

        var chosen = bestCaptures[Random.Range(0, bestCaptures.Count)];

        ExecuteMove(chosen.piece, chosen.target);

        EarnPointsForCapturing(chosen.piece.Definition);
        return true;
    }

    private bool TryPromote(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (piece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
                return true;
            }
        }

        return false;
    }

    private void DoRandomMove(List<Piece> pieces)
    {
        List<(Piece piece, Tile tile)> moves = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
            {
                moves.Add((piece, tile));
            }
        }

        if (moves.Count == 0)
            return;

        var chosen = moves[Random.Range(0, moves.Count)];
        ExecuteMove(chosen.piece, chosen.tile);
    }

    private void ExecuteMove(Piece piece, Tile target)
    {
        GameStateController.Instance.IsBusy = true;

        Tile origin = piece.currentTile;

        if (target.IsOccupied) Destroy(target.piece.gameObject);

        origin.Clear();
        target.SetPiece(piece);

        piece.MoveToTile(target, 0.35f, () =>
        {
            if (piece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
            }
            else
            {
                GameStateController.Instance.IsBusy = false;
                GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
            }
        });
    }

    private void EarnPointsForCapturing(PieceDefinitionSO def)
    {
        currentPoints += def.cost - 1;
    }

    #region Placement
    public void StartPlacement()
    {
        currentPoints = phaseSO.startingPoints;

        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        while (currentPoints > 0 && freeTiles.Count > 0)
        {
            PieceDefinitionSO pieceDef = ChoosePiece();
            if (pieceDef == null) break;

            Tile tile = ChooseTile(freeTiles, pieceDef);
            if (tile == null) break;

            Piece piece = PlacePiece(tile, pieceDef, false);
            freeTiles.Remove(tile);
            placedPieces.Add(piece);
        }

        GameStateController.Instance.SetPhase(GamePhase.PlayerPlacement);
    }

    private PieceDefinitionSO ChoosePiece()
    {
        List<PieceDefinitionSO> possible = phaseSO.availablePieces.FindAll(p => p.cost <= currentPoints);

        if (possible.Count == 0)
            return null;

        return possible[Random.Range(0, possible.Count)];
    }

    private Tile ChooseTile(List<Tile> freeTiles, PieceDefinitionSO piece)
    {
        if (piece.type == PieceType.Pawn)
        {
            List<Tile> pawnTiles = freeTiles.FindAll(t =>
                t.pos.y >= 3 && t.pos.y <= 6
            );

            if (pawnTiles.Count > 0)
                return pawnTiles[Random.Range(0, pawnTiles.Count)];
            else
                return null;
        }

        return freeTiles[Random.Range(0, freeTiles.Count)];
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool show)
    {
        currentPoints -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, false);
        tile.SetPiece(piece);

        piece.SetVisible(show);
        tile.SetOccupiedMarker(!show);

        return piece;
    }

    private bool TryPlacementDuringTurn()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        if (freeTiles.Count == 0)
            return false;

        PieceDefinitionSO pieceDef = ChoosePiece();
        if (pieceDef == null)
            return false;

        Tile tile = ChooseTile(freeTiles, pieceDef);
        if (tile == null)
            return false;

        PlacePiece(tile, pieceDef, true);

        GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
        return true;
    }
    #endregion
}
