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

        if (newPhase == GamePhase.OpponentPlacement)
            iaPlacement.StartPlacement();
        else if (newPhase == GamePhase.PlayerPlacement)
            StartFirstPlayerTurn();
    }

    private void StartFirstPlayerTurn()
    {
        RevealIAPieces();
        SetPhase(GamePhase.PlayerTurn);
    }

    private void RevealIAPieces()
    {
        foreach (var piece in FindObjectsOfType<Piece>())
        {
            if (piece.isFromPlayer)
            {
                piece.SetVisible(true);
                piece.currentTile.SetOccupiedMarker(false);
            }
        }
    }
}
