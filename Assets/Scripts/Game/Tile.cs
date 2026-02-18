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
        if (GameStateController.Instance.CurrentPhase == GamePhase.PlayerPlacement)
        {
            var selected = PlayerController.Instance.SelectedPiecePlacement;
            if (selected == null) return;

            PlayerController.Instance.TryPlacePiece(this, selected);
        }
        else if(GameStateController.Instance.CurrentPhase == GamePhase.PlayerTurn)
        {
            if(PlayerController.Instance.SelectedPiecePlacement == null)
            {
                PlayerController.Instance.OnTileClicked(this);
            }
            else
            {
                var selected = PlayerController.Instance.SelectedPiecePlacement;
                if (selected == null) return;

                PlayerController.Instance.TryPlacePiece(this, selected);
            }
        }
    }

    public void Init(Color defaultColor)
    {
        this.defaultColor = defaultColor;
    }

    public void SetOccupiedMarker(bool occupied)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, occupied ? Color.red : defaultColor);
    }

    public void SetHighlight(bool active)
    {
        BoardColorApplier.Instance.ApplyColorToSlot(tileRenderer, active ? Color.green : defaultColor);
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

        SetHighlight(false);
    }
}
