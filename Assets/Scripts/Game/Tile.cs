using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;

    [Header("Board Position")]
    public Vector2 pos;

    [Header("State")]
    public Piece piece { get; private set; }

    public bool IsOccupied => piece != null;

    //Color
    private Color defaultColor;

    private void OnMouseDown()
    {
        if (GameStateController.Instance.CurrentPhase != GamePhase.PlayerPlacement) return;

        var selected = PlacementController.Instance.selectedPiece;
        if (selected == null) return;

        PlacementController.Instance.TryPlacePiece(this, selected);
    }

    public void Init(Color defaultColor)
    {
        this.defaultColor = defaultColor;
    }

    public void SetSelected(bool selected)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, selected ? Color.yellow : defaultColor);
    }

    public void SetOccupiedMarker(bool occupied)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, occupied ? Color.red : defaultColor);
    }

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
