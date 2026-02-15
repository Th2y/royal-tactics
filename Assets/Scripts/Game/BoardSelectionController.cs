using UnityEngine;

public class BoardSelectionController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public Tile currentSelectedTile {  get; private set; }

    public static BoardSelectionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SelectTile(Tile tile)
    {
        if (!GameStateController.Instance.IsPlayerTurn)
            return;

        ClearSelection();

        if (currentSelectedTile == tile)
        {
            currentSelectedTile = null;
            return;
        }

        currentSelectedTile = tile;
        currentSelectedTile.SetSelected(true);
    }

    public void ClearSelection()
    {
        if (currentSelectedTile == null) return;

        currentSelectedTile.SetSelected(false);
        currentSelectedTile = null;
    }
}
