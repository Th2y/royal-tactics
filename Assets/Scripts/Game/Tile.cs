using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Board Position")]
    public int x;
    public int y;

    [Header("State")]
    public Piece piece {  get; private set; }

    public bool IsOccupied => piece != null;

    public void SetPiece(Piece newPiece)
    {
        piece = newPiece;
        newPiece.SetTile(this);
    }

    public void Clear()
    {
        piece = null;
    }
}
