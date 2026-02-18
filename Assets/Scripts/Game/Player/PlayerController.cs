using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnityMethods
{
    public PieceDefinitionSO SelectedPiecePlacement { get; private set; }

    public event Action ShowPlacementButtons;
    private List<Piece> placedPieces = new();

    public event Action<int> OnCoinsChanged;

    private int currentCoins;
    public int CurrentCoins
    {
        get => currentCoins;
        set
        {
            if (currentCoins == value)
                return;

            currentCoins = value;
            OnCoinsChanged?.Invoke(currentCoins);
        }
    }
    public event Action<int> OnTotalCoinsChanged;

    private int totalCoins;
    public int TotalCoins
    {
        get => totalCoins;
        set
        {
            if (totalCoins == value)
                return;

            totalCoins = value;
            OnTotalCoinsChanged?.Invoke(totalCoins);
        }
    }

    public Piece SelectedPiece { get; private set; }
    private List<Tile> highlightedTiles = new();

    [HideInInspector] public bool CanMove = false;

    public static PlayerController Instance { get; private set; }

    public override InitPriority Priority => InitPriority.PlayerController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitCoins();
    }

    public override void InitStart()
    {
        PlayerUI.Instance.PlayerDoAnything += DisableMovement;
    }

    private void OnDestroy()
    {
        PlayerUI.Instance.PlayerDoAnything -= DisableMovement;
    }

    public void InitCoins()
    {
        CurrentCoins = PhaseController.Instance.CurrentPhase.startingPoints;
        TotalCoins = CurrentCoins;
    }

    private void DisableMovement()
    {
        CanMove = false;
        PlayerUI.Instance.RefreshButtons();
    }

    public bool CheckCanDoAnything()
    {
        if (CanCapture()) return true;

        if (CanPromote()) return true;

        if (CanPlaceAnyPiece()) return true;

        if (CheckCanMove()) return true;

        return false;
    }

    public void OnPieceClicked(Piece piece)
    {
        if (!GameStateController.Instance.IsPlayerTurn) return;

        if (!piece.isFromPlayer) return;

        if(!CanMove) return;

        ClearSelection();

        SelectedPiece = piece;
        SelectedPiece.SetSelected(true);

        HighlightValidTiles(piece);
    }

    private void ClearSelection()
    {
        if (SelectedPiece != null)
            SelectedPiece.SetSelected(false);

        foreach (Tile tile in highlightedTiles)
            tile.SetIsValid(false);

        highlightedTiles.Clear();
        SelectedPiece = null;
    }

    public void OnTileClicked(Tile tile)
    {
        if (SelectedPiece == null)
            return;

        if (!highlightedTiles.Contains(tile))
            return;

        ExecuteMove(tile);
    }

    private void HighlightValidTiles(Piece piece)
    {
        highlightedTiles.Clear();

        foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
        {
            tile.SetIsValid(true);
            highlightedTiles.Add(tile);
        }

        foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
        {
            if (!highlightedTiles.Contains(tile))
            {
                tile.SetIsValid(true);
                highlightedTiles.Add(tile);
            }
        }
    }

    #region Capture
    private bool CanCapture()
    {
        foreach (Piece piece in placedPieces)
        {
            if (piece == null) continue;

            if (piece.GetValidCaptures(BoardController.Instance).Count > 0)
                return true;
        }

        return false;
    }
    #endregion

    #region Promotion
    private bool CanPromote()
    {
        foreach (Piece piece in placedPieces)
        {
            if (piece is Pawn pawn && pawn.CanPromote)
                return true;
        }

        return false;
    }
    #endregion

    #region Placement
    private bool CanPlaceAnyPiece()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        if (freeTiles.Count == 0)
            return false;

        foreach (PieceDefinitionSO def in PhaseController.Instance.CurrentPhase.availablePieces)
        {
            if (!HaveEnoughCoinsToPlace(def))
                continue;

            foreach (Tile tile in freeTiles)
            {
                if (IsValidPlacement(tile, def))
                    return true;
            }
        }

        return false;
    }

    public bool HaveEnoughCoinsToPlace(PieceDefinitionSO def)
    {
        return CurrentCoins >= def.cost;
    }

    public void SelectPiecePlacement(PieceDefinitionSO def)
    {
        SelectedPiecePlacement = def;
        ClearSelection();
    }

    public void TryPlacePiece(Tile tile)
    {
        if (tile == null || SelectedPiecePlacement == null) return;

        if (!GameStateController.Instance.IsPlayerTurn) return;

        if (tile.IsOccupied) return;

        if (!HaveEnoughCoinsToPlace(SelectedPiecePlacement)) return;

        if (!IsValidPlacement(tile, SelectedPiecePlacement)) return;

        PlacePiece(tile, SelectedPiecePlacement);
    }

    private void PlacePiece(Tile tile, PieceDefinitionSO def)
    {
        CurrentCoins -= def.cost;
        DisableMovement();

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, true);
        tile.SetPiece(piece);
        placedPieces.Add(piece);

        ShowPlacementButtons?.Invoke();
        SelectPiecePlacement(null);
        PlayerUI.Instance.PlayerDoAnything?.Invoke();
    }

    private bool IsValidPlacement(Tile tile, PieceDefinitionSO def)
    {
        if (def.type == PieceType.Pawn)
        {
            return tile.pos.y >= 1 && tile.pos.y <= 4;
        }

        return true;
    }
    #endregion

    #region Move
    private bool CheckCanMove()
    {
        foreach (Piece piece in placedPieces)
        {
            if (piece == null) continue;

            if (piece.GetValidMoves(BoardController.Instance).Count > 0)
                return true;
        }

        return false;
    }

    private void ExecuteMove(Tile target)
    {
        Tile origin = SelectedPiece.currentTile;

        if (target.IsOccupied)
        {
            EarnPointsForCapturing(target.Piece.Definition);
            target.Piece.OnCaptured();
        }

        origin.Clear();
        target.SetPiece(SelectedPiece);

        PlayerUI.Instance.PlayerDoAnything?.Invoke();

        SelectedPiece.MoveToTile(target, 0.35f, () =>
        {
            if (SelectedPiece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
            }
        });

        ClearSelection();
    }
    #endregion

    public void RemovePiece(Piece piece)
    {
        placedPieces.Remove(piece);
        TotalCoins -= piece.Definition.cost;
    }

    private void EarnPointsForCapturing(PieceDefinitionSO def)
    {
        int value = def.cost;
        CurrentCoins += value;
        TotalCoins += value;
    }
}
