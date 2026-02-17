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
    [SerializeField] private AIPlacementController aiPlacement;
    [SerializeField] private AIController aiController;

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
                aiPlacement.StartPlacement();
                break;
            case GamePhase.PlayerPlacement:
                PlacementUI.Instance.RefreshButtons();
                break;
            case GamePhase.OpponentTurn:
                Invoke(nameof(AIWaitAndPlay), 5f);
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

    private void AIWaitAndPlay()
    {
        aiController.PlayTurn();
        SetPhase(GamePhase.PlayerTurn);
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
