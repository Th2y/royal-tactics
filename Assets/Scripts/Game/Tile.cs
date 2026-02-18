using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;
    public Vector2 pos;

    public Piece Piece { get; private set; }
    public bool IsOccupied => Piece != null;

    private Color defaultColor;
    private Color lastColor;
    public bool IsValid { get; private set; } = false;
    private bool canInteract = false;

    private void OnMouseDown()
    {
        if (!canInteract) return;

        var player = HumanPlayerController.Instance;

        if (player.SelectedPiecePlacement != null)
        {
            player.TryPlacePiece(this);
        }
        else
        {
            player.OnTileClicked(this);
        }
    }

    private void OnMouseEnter()
    {
        var gameState = GameStateController.Instance.CurrentPhase;
        var player = HumanPlayerController.Instance;

        bool hasPlacementSelected = player.SelectedPiecePlacement != null;
        bool hasPieceSelected = player.SelectedPiece != null;

        canInteract =
            (gameState == GamePhase.PlayerTurn &&
                (
                    (hasPieceSelected && IsValid) ||
                    hasPlacementSelected
                )
            )
            ||
            (gameState == GamePhase.PlayerPlacement &&
                !IsOccupied &&
                hasPlacementSelected);

        if (canInteract)
        {
            if (player.SelectedPiecePlacement != null && player.SelectedPiecePlacement.type == PieceType.Pawn)
            {
                canInteract = pos.y >= 1 && pos.y <= 4;
            }
        }

        if (canInteract)
        {
            SetPressed(true);
        }
    }

    private void OnMouseExit()
    {
        SetPressed(false);
    }

    public void Init(Color color)
    {
        defaultColor = color;
        lastColor = color;
    }

    public void SetPressed(bool pressed)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, pressed ? Color.yellow : lastColor);
    }

    public void SetOccupiedMarker(bool occupied)
    {
        lastColor = occupied ? Color.red : defaultColor;
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, lastColor);
    }

    public void SetIsValid(bool valid)
    {
        IsValid = valid;
        lastColor = valid ? Color.green : defaultColor;
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, lastColor);
    }

    public void SetPiece(Piece newPiece)
    {
        Piece = newPiece;
        newPiece.SetTile(this);
    }

    public void Clear()
    {
        Piece = null;
    }

    public void ClearAndDestroyPiece()
    {
        if (Piece != null)
        {
            Destroy(Piece.gameObject);
            Piece = null;
        }

        SetIsValid(false);
    }
}
