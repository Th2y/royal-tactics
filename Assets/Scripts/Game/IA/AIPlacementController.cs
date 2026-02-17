using UnityEngine;
using System.Collections.Generic;

public class AIPlacementController : MonoBehaviour
{
    [SerializeField] private BoardController board;
    [SerializeField] private PhaseSO phaseSO;

    public List<Piece> placedPieces { get; private set; } = new();

    private int currentPoints;

    public void StartPlacement()
    {
        currentPoints = phaseSO.startingPoints;

        List<Tile> freeTiles = board.GetAllFreeTiles();

        while (currentPoints > 0 && freeTiles.Count > 0)
        {
            PieceDefinitionSO pieceDef = ChoosePiece();
            if (pieceDef == null) break;

            Tile tile = ChooseTile(freeTiles, pieceDef);
            if(tile == null) break;

            Piece piece = PlacePiece(tile, pieceDef);
            freeTiles.Remove(tile);
            placedPieces.Add(piece);
        }

        FinishPlacement();
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

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def)
    {
        currentPoints -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, false);
        tile.SetPiece(piece);

        piece.SetVisible(false);
        tile.SetOccupiedMarker(true);

        return piece;
    }

    private void FinishPlacement()
    {
        GameStateController.Instance.SetPhase(GamePhase.PlayerPlacement);
    }
}
