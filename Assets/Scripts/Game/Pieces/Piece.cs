using System.Collections.Generic;
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
    [SerializeField] private ModelColorApplier modelColorApplier;
    public PieceDefinitionSO Definition { get; private set; }
    protected Renderer[] renderers;
    public Tile currentTile {  get; private set; }
    public bool isFromPlayer {  get; private set; }
    public virtual bool CanPromote => false;

    public virtual void Initialize(PieceDefinitionSO def, bool isFromPlayer)
    {
        Definition = def;
        renderers = GetComponentsInChildren<Renderer>();
        this.isFromPlayer = isFromPlayer;

        modelColorApplier.isPlayer = isFromPlayer;
        modelColorApplier.Init();
    }

    public void SetTile(Tile tile)
    {
        currentTile = tile;
        transform.position = new Vector3(tile.transform.position.x, 0.415f, tile.transform.position.z);
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in renderers)
            r.enabled = visible;
    }

    public abstract List<Tile> GetValidMoves(BoardController board);
    public abstract List<Tile> GetValidCaptures(BoardController board);
}
