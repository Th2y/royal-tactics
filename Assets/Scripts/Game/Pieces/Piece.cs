using UnityEngine;

public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen
}

public enum PlayerOwner
{
    Player,
    Opponent
}

public abstract class Piece : MonoBehaviour
{
    public PieceDefinitionSO Definition { get; private set; }
    protected Tile currentTile;

    public void Initialize(PieceDefinitionSO def)
    {
        Definition = def;
    }

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        transform.position = new Vector3(tile.transform.position.x, 0.415f, tile.transform.position.z);
    }
}
