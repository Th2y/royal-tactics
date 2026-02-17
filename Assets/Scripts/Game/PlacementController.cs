using UnityEngine;
using System.Collections.Generic;
using System;

public class PlacementController : UnityMethods
{
    [Header("Piece Definitions")]
    [SerializeField] private PhaseSO phaseSO;
    public PieceDefinitionSO selectedPiece { get; private set; }

    public event Action OnPointsChanged;

    private int currentPoints;

    public static PlacementController Instance { get; private set; }

    public override InitPriority priority => InitPriority.PlacementController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentPoints = phaseSO.startingPoints;
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

        OnPointsChanged?.Invoke();

        if(currentPoints == 0)
        {
            GameStateController.Instance.BeforeFirstPlayerTurn();
        }
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
        selectedPiece = def;
    }
}
