using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;
    [SerializeField] private Collider colider;
    public TileName TileName { get; private set; }
    public Vector2Int Position;

    public Piece Piece { get; private set; }
    public bool IsOccupied => Piece != null;

    private Color defaultColor;
    private Color lastColor;

    public bool IsValid { get; private set; }
    private bool canInteract;

    private void Awake()
    {
        TileName = (TileName)(Position.x * 8 + Position.y);

        HandleCollider(false);
    }

    public void Init(Color color)
    {
        defaultColor = color;
        lastColor = color;
    }

    public void HandleCollider(bool enabled)
    {
        colider.enabled = enabled;
    }

    #region Input
    public void OnHoverEnter()
    {
        EvaluateInteraction();

        if (canInteract)
            SetPressed(true);
    }

    public void OnHoverExit()
    {
        SetPressed(false);
    }

    public void OnClick()
    {
        if (!canInteract) return;

        var player = HumanPlayerController.Instance;

        if (player.SelectedPiecePlacement != null)
            player.TryPlacePiece(this);
        else if (player.SelectedPiece != null)
            player.OnTileClicked(this);
        else
            player.OnPieceClicked(Piece);
    }
    #endregion

    #region Logic
    private void EvaluateInteraction()
    {
        var gameState = ChooseGameMode.Instance.CurrentGameMode.CurrentPhase;
        var player = HumanPlayerController.Instance;

        bool hasPlacementSelected = player.SelectedPiecePlacement != null;
        bool withPieceSelected = player.SelectedPiece != null && IsValid;
        bool withoutPieceSelected = player.SelectedPiece == null && Piece != null && Piece.IsFromPlayer;

        canInteract =
            (gameState == GameTurn.PlayerTurn && player.CanMove &&
                (withPieceSelected || hasPlacementSelected || withoutPieceSelected))
            ||
            (gameState == GameTurn.PlayerPlacement &&
                !IsOccupied && hasPlacementSelected);

        if (canInteract && hasPlacementSelected && player.SelectedPiecePlacement.type == PieceType.Pawn)
        {
            canInteract = Position.y >= 1 && Position.y <= 4;
        }
    }
    #endregion

    #region Feedback
    public void SetPressed(bool pressed)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, pressed ? Color.yellow : lastColor);

        if (Piece != null) Piece.SetPressed(pressed);
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
    #endregion

    #region Piece
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
            Destroy(Piece.gameObject);

        Piece = null;
        SetIsValid(false);
    }
    #endregion
}
