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

public class GameStateController : UnityMethods
{
    [SerializeField] private UIGameController uiGameController;
    [SerializeField] private AIController aiController;

    public bool IsBusy;
    public GamePhase CurrentPhase { get; private set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GamePhase.PlayerTurn ||
        CurrentPhase == GamePhase.PlayerPlacement;

    public static GameStateController Instance { get; private set; }

    public override InitPriority priority => InitPriority.GameStateController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void InitStart()
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
                aiController.StartPlacement();
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
        foreach (var piece in aiController.placedPieces)
        {
            piece.SetVisible(true);
            piece.currentTile.SetOccupiedMarker(false);
        }
    }
}
