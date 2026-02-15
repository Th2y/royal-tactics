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
    public PieceType type;
    public int cost;
    [HideInInspector] public PlayerOwner owner;

    public Tile currentTile {  get; protected set; }

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        transform.position = tile.transform.position;
    }
}

