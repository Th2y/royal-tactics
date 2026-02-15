using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Renderer tileRenderer;

    [Header("Board Position")]
    public int x;
    public int y;

    [Header("State")]
    public Piece piece {  get; private set; }

    public bool IsOccupied => piece != null;

    //Color
    private BoardColorApplier boardColorApplier;
    private Color defaultColor;

    private void OnMouseDown()
    {
        BoardSelectionController.Instance.SelectTile(this);
    }

    public void Init(BoardColorApplier boardColorApplier, Color defaultColor)
    {
        this.boardColorApplier = boardColorApplier;
        this.defaultColor = defaultColor;
    }

    public void SetSelected(bool selected)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, selected ? Color.yellow : defaultColor);
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
