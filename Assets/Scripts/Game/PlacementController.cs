using UnityEngine;
using System.Collections.Generic;

public class PlacementController : MonoBehaviour
{
    [Header("Piece Definitions")]
    [SerializeField] private PhaseSO phaseSO;

    private int currentPoints;

    public static PlacementController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentPoints = phaseSO.startingPoints;
    }

    public bool CanPlace(PieceDefinitionSO def)
    {
        return currentPoints >= def.cost && IsUnlocked(def);
    }

    public void TryPlacePiece(Tile tile, PieceDefinitionSO def)
    {
        if (!GameStateController.Instance.IsPlayerTurn)
            return;

        if (tile.IsOccupied)
            return;

        if (!CanPlace(def))
            return;

        PlacePiece(tile, def);
    }

    private void PlacePiece(Tile tile, PieceDefinitionSO def)
    {
        currentPoints -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, true);
        tile.SetPiece(piece);

        Debug.LogError($"Placed {def.type} | Points left: {currentPoints}");
    }

    private bool IsUnlocked(PieceDefinitionSO def)
    {
        if (def.unlockedByDefault) return true;

        return false;
    }
}
