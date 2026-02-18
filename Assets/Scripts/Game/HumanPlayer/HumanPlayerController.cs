using System;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerController : BasePlayerController<HumanPlayerController>
{
    public PieceDefinitionSO SelectedPiecePlacement { get; private set; }

    public event Action ShowPlacementButtons;

    public event Action<int> OnCoinsChanged;

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

    public Piece SelectedPiece { get; private set; }
    private List<Tile> highlightedTiles = new();

    [HideInInspector] public bool CanMove = false;
    [HideInInspector] public bool IsInPromotion = false;

    #region Unity Default Methods
    public override void OnInitAwake()
    {
        base.OnInitAwake();
    }

    public override void OnInitStart()
    {
        base.OnInitStart();
        HumanPlayerUI.Instance.PlayerDoAnything += DisableMovement;
    }

    private void OnDestroy()
    {
        HumanPlayerUI.Instance.PlayerDoAnything -= DisableMovement;
    }
    #endregion

    #region Check If Can Do
    public override bool CheckCanDoAnything()
    {
        if (CheckCanCapture(null)) return true;

        if (CheckCanPromote(null)) return true;

        if (CheckCanPlaceAnyPiece()) return true;

        if (CheckCanMove(null)) return true;

        return false;
    }

    protected override bool CheckCanCapture(List<Piece> pieces)
    {
        foreach (Piece piece in PlacedPieces)
        {
            if (piece == null) continue;

            if (piece.GetValidCaptures(BoardController.Instance).Count > 0)
                return true;
        }

        return false;
    }

    protected override bool CheckCanPromote(List<Piece> pieces)
    {
        foreach (Piece piece in PlacedPieces)
        {
            if (piece is Pawn pawn && pawn.CanPromote)
                return true;
        }

        return false;
    }

    protected override bool CheckCanPlaceAnyPiece()
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

    protected override bool CheckCanMove(List<Piece> pieces)
    {
        foreach (Piece piece in PlacedPieces)
        {
            if (piece == null) continue;

            if (piece.GetValidMoves(BoardController.Instance).Count > 0)
                return true;
        }

        return false;
    }
    #endregion

    #region Selection
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

    public void SelectPiecePlacement(PieceDefinitionSO def)
    {
        SelectedPiecePlacement = def;
        ClearSelection();
    }
    #endregion

    #region Actions
    private void DisableMovement()
    {
        CanMove = false;
        HumanPlayerUI.Instance.RefreshButtons();
    }

    public bool HaveEnoughCoinsToPlace(PieceDefinitionSO def)
    {
        return CurrentCoins >= def.cost;
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
        currentCoins -= def.cost;
        DisableMovement();

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, true);
        tile.SetPiece(piece);
        PlacedPieces.Add(piece);

        ShowPlacementButtons?.Invoke();
        SelectPiecePlacement(null);
        HumanPlayerUI.Instance.PlayerDoAnything?.Invoke();
    }

    private bool IsValidPlacement(Tile tile, PieceDefinitionSO def)
    {
        if (def.type == PieceType.Pawn)
        {
            return tile.pos.y >= 1 && tile.pos.y <= 4;
        }

        return true;
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

        HumanPlayerUI.Instance.PlayerDoAnything?.Invoke();

        SelectedPiece.MoveToTile(target, 0.35f, () =>
        {
            if (SelectedPiece is Pawn pawn && pawn.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(pawn);
                ClearSelection();
            }
            else
            {
                ClearSelection();
            }
        });
    }
    #endregion
}
