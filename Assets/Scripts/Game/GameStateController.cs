using UnityEngine;

public enum GamePhase
{
    OpponentPlacement,
    PlayerPlacement,
    OpponentTurn,
    PlayerTurn,
    GameOver
}

public class GameStateController : MonoBehaviour
{
    [SerializeField] private IAPlacementController iaPlacement;

    public GamePhase CurrentPhase { get; private set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GamePhase.PlayerTurn ||
        CurrentPhase == GamePhase.PlayerPlacement;

    public static GameStateController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetPhase(GamePhase.OpponentPlacement);
    }

    public void SetPhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        Debug.LogError("FASE ATUAL: " + newPhase);

        switch (newPhase)
        {
            case GamePhase.OpponentPlacement:
                iaPlacement.StartPlacement();
                break;
            case GamePhase.PlayerPlacement:
                PlacementUI.Instance.RefreshButtons();
                break;
            default:
                break;

        }
    }

    public void BeforeFirstPlayerTurn()
    {
        RevealIAPieces();
        SetPhase(GamePhase.OpponentTurn);
    }

    private void RevealIAPieces()
    {
        foreach (var piece in iaPlacement.placedPieces)
        {
            piece.SetVisible(true);
            piece.currentTile.SetOccupiedMarker(false);
        }
    }
}
