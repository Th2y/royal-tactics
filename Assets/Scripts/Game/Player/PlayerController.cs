using System;
using System.Collections.Generic;

public class PlayerController : UnityMethods
{
    public PieceDefinitionSO SelectedPiece { get; private set; }

    public event Action ShowPlacementButtons;
    private List<Piece> placedPieces = new();

    private int currentPoints;
    public int TotalPoints {  get; private set; }

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

        currentPoints = GameStateController.Instance.PhaseSO.startingPoints;
        TotalPoints = currentPoints;
    }

    public override void InitStart()
    {

    }

    public bool CanPlace(PieceDefinitionSO def)
    {
        return currentPoints >= def.cost;
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
        currentPoints -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, true);
        tile.SetPiece(piece);
        placedPieces.Add(piece);

        ShowPlacementButtons?.Invoke();
        SelectPiece(null);
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

    public void EarnPointsForCapturing(PieceDefinitionSO def)
    {
        int value = def.cost - 1;
        currentPoints += value;
        TotalPoints += value;
    }
}
