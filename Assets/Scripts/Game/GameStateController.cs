using UnityEngine;

public enum GamePhase
{
    OpponentPlacement,
    PlayerPlacement,
    OpponentTurn,
    PlayerTurn,
    GameOverWin,
    GameOverLost
}

public class GameStateController : MonoBehaviour
{
    [SerializeField] private UIGameController uiGameController;
    [SerializeField] private AIPlacementController aiPlacement;
    [SerializeField] private AIController aiController;

    public bool IsBusy;
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
        uiGameController.SetGamePhaseText(newPhase);

        switch (newPhase)
        {
            case GamePhase.OpponentPlacement:
                aiPlacement.StartPlacement();
                break;
            case GamePhase.PlayerPlacement:
                PlacementUI.Instance.RefreshButtons();
                break;
            case GamePhase.OpponentTurn:
                aiController.Play();
                break;
            case GamePhase.PlayerTurn:
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
        foreach (var piece in aiPlacement.placedPieces)
        {
            piece.SetVisible(true);
            piece.currentTile.SetOccupiedMarker(false);
        }
    }
}
