using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : BasePlayerController<AIController>
{
    private Tile[] tiles;

    #region Unity Default Methods
    public override void OnInitAwake()
    {
        base.OnInitAwake();
    }

    public override void OnInitStart()
    {
        base.OnInitStart();
    }
    #endregion

    #region Check If Can Do
    public override bool CheckCanDoAnything()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (CheckCanCapture(aiPieces)) return true;

        if (CheckCanPromote(aiPieces)) return true;

        if (CheckCanPlaceAnyPiece()) return true;

        if (CheckCanMove(aiPieces)) return true;

        return false;
    }

    protected override bool CheckCanCapture(List<Piece> pieces)
    {
        List<(Piece piece, Tile target, int value)> captures = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
            {
                Piece targetPiece = tile.Piece;
                int value = targetPiece.Definition.cost;

                captures.Add((piece, tile, value));
            }
        }

        return captures.Count > 0;
    }

    protected override bool CheckCanPromote(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (piece is Pawn pawn && pawn.CanPromote)
            {
                return true;
            }
        }

        return false;
    }

    protected override bool CheckCanPlaceAnyPiece()
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

        return true;
    }

    protected override bool CheckCanMove(List<Piece> pieces)
    {
        List<(Piece piece, Tile tile)> moves = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
            {
                moves.Add((piece, tile));
            }
        }

        return moves.Count > 0;
    }
    #endregion

    #region Actions
    public void Play()
    {
        Invoke(nameof(PlayTurn), 5f);
    }

    private void PlayTurn()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (TryCapture(aiPieces)) return;

        if (TryPromote(aiPieces)) return;

        if (currentCoins > 0 && TryPlacementDuringTurn()) return;

        DoRandomMove(aiPieces);
    }

    private List<Piece> GetAIPieces()
    {
        List<Piece> pieces = new();

        if (tiles == null || tiles.Length <= 0)
        {
            tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        }

        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && !tile.Piece.isFromPlayer)
                pieces.Add(tile.Piece);
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
                Piece targetPiece = tile.Piece;
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

        var chosen = bestCaptures[UnityEngine.Random.Range(0, bestCaptures.Count)];

        ExecuteMove(chosen.piece, chosen.target);

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

        var chosen = moves[UnityEngine.Random.Range(0, moves.Count)];
        ExecuteMove(chosen.piece, chosen.tile);
    }

    private void ExecuteMove(Piece piece, Tile target)
    {
        Tile origin = piece.currentTile;

        if (target.IsOccupied)
        {
            EarnPointsForCapturing(target.Piece.Definition);
            target.Piece.OnCaptured();
        }

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
                GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
            }
        });
    }

    public void StartPlacement()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        while (currentCoins > 0 && freeTiles.Count > 0)
        {
            PieceDefinitionSO pieceDef = ChoosePiece();
            if (pieceDef == null) break;

            Tile tile = ChooseTile(freeTiles, pieceDef);
            if (tile == null) break;

            Piece piece = PlacePiece(tile, pieceDef, false);
            freeTiles.Remove(tile);
            PlacedPieces.Add(piece);
        }

        GameStateController.Instance.SetPhase(GamePhase.PlayerPlacement);
    }

    private PieceDefinitionSO ChoosePiece()
    {
        List<PieceDefinitionSO> possible = PhaseController.Instance.CurrentPhase.availablePieces.FindAll(p => p.cost <= currentCoins);

        if (possible.Count == 0)
            return null;

        return possible[UnityEngine.Random.Range(0, possible.Count)];
    }

    private Tile ChooseTile(List<Tile> freeTiles, PieceDefinitionSO piece)
    {
        if (piece.type == PieceType.Pawn)
        {
            List<Tile> pawnTiles = freeTiles.FindAll(t =>
                t.pos.y >= 3 && t.pos.y <= 6
            );

            if (pawnTiles.Count > 0)
                return pawnTiles[UnityEngine.Random.Range(0, pawnTiles.Count)];
            else
                return null;
        }

        return freeTiles[UnityEngine.Random.Range(0, freeTiles.Count)];
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool show)
    {
        currentCoins -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, false);
        tile.SetPiece(piece);

        piece.SetVisible(show);
        tile.SetOccupiedMarker(!show);

        return piece;
    }
    #endregion
}
