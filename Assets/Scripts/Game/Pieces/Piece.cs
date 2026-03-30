using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Piece : MonoBehaviour
{
    [SerializeField] private ModelColorApplier modelColorApplier;
    protected Renderer[] renderers;
    public Tile CurrentTile;

#if UNITY_EDITOR
    public bool IsFromPlayer;
    public PieceDefinitionSO Definition;
#else
    public bool IsFromPlayer {  get; private set; }
    public PieceDefinitionSO Definition { get; private set; }
#endif

    public virtual bool CanPromote
    {
        get { return false; }
    }

    public virtual void Initialize(PieceDefinitionSO def, bool isFromPlayer)
    {
        Definition = def;
        renderers = GetComponentsInChildren<Renderer>();
        this.IsFromPlayer = isFromPlayer;

        modelColorApplier.isPlayer = isFromPlayer;
        modelColorApplier.OnInitAwake();
    }

    public void SetTile(Tile tile)
    {
        CurrentTile = tile;
        transform.position = new Vector3(tile.transform.position.x, 0.415f, tile.transform.position.z);
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in renderers)
            r.enabled = visible;
    }

    public void SetSelected(bool selected)
    {
        modelColorApplier.SetEmissionColor(selected);
    }

    public void SetPressed(bool pressed)
    {
        if (pressed)
        {
            modelColorApplier.SetColor(Color.yellow, false);
        }
        else
        {
            modelColorApplier.SetColor(modelColorApplier.LastColor);
        }
    }

    public void MoveToTile(Tile tile, float duration, System.Action onComplete = null)
    {
        CurrentTile = tile;

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

    public void OnCaptured()
    {
        if (IsFromPlayer)
        {
            HumanPlayerController.Instance.RemovePiece(this);
        }
        else
        {
            AIController.Instance.RemovePiece(this);
        }

        Destroy(gameObject);
    }

    public virtual List<Tile> FilterValidSpawnTiles(List<Tile> freeTiles, bool isHuman)
    {
        return freeTiles;
    }

    public virtual bool IsValidPlacement(Tile tile, bool isHuman)
    {
        return true;
    }

    public abstract List<Tile> GetValidMoves(BoardController board);
    public abstract List<Tile> GetValidCaptures(BoardController board, bool fromSamePlayer = false);
    public virtual List<Tile> GetPiecesDefending(BoardController board)
    {
        List<Tile> tiles = new();

        foreach (var piece in board.GetAllPieces())
        {
            if (piece.IsFromPlayer != IsFromPlayer) continue;

            if (piece.GetValidCaptures(board, true).Contains(CurrentTile)) tiles.Add(piece.CurrentTile);
        }

        return tiles;
    }

    public virtual List<Tile> GetAttackTiles(BoardController board)
    {
        List<Tile> attacks = new();

        attacks.AddRange(GetValidMoves(board));
        attacks.AddRange(GetValidCaptures(board));

        return attacks;
    }
}
