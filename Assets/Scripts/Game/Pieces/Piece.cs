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

    public bool CanReceiveClicks = true;

    private void OnMouseDown()
    {
        if (CanReceiveClicks)
        {
            if (isFromPlayer)
            {
                PlayerController.Instance.OnPieceClicked(this);
            }
            else
            {
                PlayerController.Instance.OnTileClicked(currentTile);
            }
        }
    }

    private void OnMouseEnter()
    {
        CanReceiveClicks = false;

        var gameState = GameStateController.Instance.CurrentPhase;

        if (gameState != GamePhase.PlayerPlacement && gameState != GamePhase.PlayerTurn)
            return;

        if (isFromPlayer)
        {
            var player = PlayerController.Instance;

            CanReceiveClicks =
                player.CanMove &&
                player.SelectedPiece == null &&
                player.SelectedPiecePlacement == null;
        }
        else
        {
            CanReceiveClicks = currentTile.IsValid;
        }

        if (CanReceiveClicks)
        {
            SetPressed(true);
            if (currentTile  != null) currentTile.SetPressed(true);
        }
    }

    private void OnMouseExit()
    {
        SetPressed(false);
        if (currentTile != null) currentTile.SetPressed(false);
    }

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

    public void SetSelected(bool selected)
    {
        modelColorApplier.SetColor(selected ? Color.green : modelColorApplier.DefaultColor);
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

    public void OnCaptured()
    {
        if (isFromPlayer)
        {
            PlayerController.Instance.RemovePiece(this);
        }
        else
        {
            AIController.Instance.RemovePiece(this);
        }

        Destroy(gameObject);
    }

    public abstract List<Tile> GetValidMoves(BoardController board);
    public abstract List<Tile> GetValidCaptures(BoardController board);
}
