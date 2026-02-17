using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public virtual void Initialize(PieceDefinitionSO def, bool isFromPlayer)
    {
        Definition = def;
        renderers = GetComponentsInChildren<Renderer>();
        this.isFromPlayer = isFromPlayer;

        modelColorApplier.isPlayer = isFromPlayer;
        modelColorApplier.InitAwake();
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

    public void MoveToTile(Tile tile, float duration, System.Action onComplete = null)
    {
        currentTile = tile;

        Vector3 targetPos = new Vector3(
            tile.transform.position.x,
            0.415f,
            tile.transform.position.z
        );

        transform.DOMove(targetPos, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public abstract List<Tile> GetValidMoves(BoardController board);
    public abstract List<Tile> GetValidCaptures(BoardController board);
}
