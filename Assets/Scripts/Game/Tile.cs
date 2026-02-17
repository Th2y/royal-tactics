using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;
    public Vector2 pos;

    public Piece Piece { get; private set; }
    public bool IsOccupied => Piece != null;

    private Color defaultColor;

    private void OnMouseDown()
    {
        if (GameStateController.Instance.CurrentPhase != GamePhase.PlayerPlacement) return;

        var selected = PlayerController.Instance.SelectedPiece;
        if (selected == null) return;

        PlayerController.Instance.TryPlacePiece(this, selected);
    }

    public void Init(Color defaultColor)
    {
        this.defaultColor = defaultColor;
    }

    public void SetOccupiedMarker(bool occupied)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, occupied ? Color.red : defaultColor);
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
}
