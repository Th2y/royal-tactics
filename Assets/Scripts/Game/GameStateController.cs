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
    public static GameStateController Instance { get; private set; }

    public GamePhase CurrentPhase { get; private set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GamePhase.PlayerTurn ||
        CurrentPhase == GamePhase.PlayerPlacement;

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
    }
}
