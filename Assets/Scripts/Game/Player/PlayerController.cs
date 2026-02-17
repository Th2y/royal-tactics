using System;
using System.Collections.Generic;

public class PlayerController : UnityMethods
{
    public PieceDefinitionSO SelectedPiece { get; private set; }

    public event Action ShowPlacementButtons;
    private List<Piece> placedPieces = new();

    private int currentCoins;
    public int TotalCoins {  get; private set; }

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

        currentCoins = GameStateController.Instance.PhaseSO.startingPoints;
        TotalCoins = currentCoins;
    }

    public override void InitStart()
    {

    }

    public bool CanPlace(PieceDefinitionSO def)
    {
        return currentCoins >= def.cost;
    }

    public void TryPlacePiece(Tile tile, PieceDefinitionSO def)
    {
        if (!GameStateController.Instance.IsPlayerTurn)
            return;

        if (tile.IsOccupied)
            return;

        if (!CanPlace(def))
            return;

        if (!IsValidPlacement(tile, def))
            return;

        PlacePiece(tile, def);
    }

    private void PlacePiece(Tile tile, PieceDefinitionSO def)
    {
        currentCoins -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, true);
        tile.SetPiece(piece);
        placedPieces.Add(piece);

        ShowPlacementButtons?.Invoke();
        SelectPiece(null);
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

    public void SelectPiece(PieceDefinitionSO def)
    {
        SelectedPiece = def;
    }

    public void RemovePiece(Piece piece)
    {
        placedPieces.Remove(piece);
    }

    public void EarnPointsForCapturing(PieceDefinitionSO def)
    {
        int value = def.cost - 1;
        currentCoins += value;
        TotalCoins += value;
    }
}
