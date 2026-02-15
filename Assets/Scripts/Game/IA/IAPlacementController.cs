using UnityEngine;
using System.Collections.Generic;

public class IAPlacementController : MonoBehaviour
{
    [SerializeField] private BoardController board;
    [SerializeField] private PhaseSO phaseSO;

    private int currentPoints;

    public void StartPlacement()
    {
        currentPoints = phaseSO.startingPoints;

        List<Tile> freeTiles = board.GetAllFreeTiles();

        while (currentPoints > 0 && freeTiles.Count > 0)
        {
            PieceDefinitionSO piece = ChoosePiece();
            if (piece == null) break;

            Tile tile = ChooseTile(freeTiles);

            PlacePiece(tile, piece);
            freeTiles.Remove(tile);
        }

        FinishPlacement();
    }

    private PieceDefinitionSO ChoosePiece()
    {
        List<PieceDefinitionSO> possible = phaseSO.availablePieces.FindAll(p =>
            p.cost <= currentPoints &&
            p.unlockedByDefault
        );

        if (possible.Count == 0)
            return null;

        return possible[Random.Range(0, possible.Count)];
    }

    private Tile ChooseTile(List<Tile> freeTiles)
    {
        return freeTiles[Random.Range(0, freeTiles.Count)];
    }

    private void PlacePiece(Tile tile, PieceDefinitionSO def)
    {
        currentPoints -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def);
        tile.SetPiece(piece);
    }

    private void FinishPlacement()
    {
        GameStateController.Instance.SetPhase(GamePhase.PlayerPlacement);
    }
}
