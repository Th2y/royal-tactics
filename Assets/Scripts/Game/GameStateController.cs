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
    [SerializeField] private PhaseSO phaseSO;
    public PhaseSO PhaseSO => phaseSO;

    public GamePhase CurrentPhase { get; private set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GamePhase.PlayerTurn ||
        CurrentPhase == GamePhase.PlayerPlacement;

    public static GameStateController Instance { get; private set; }

    public override InitPriority Priority => InitPriority.GameStateController;

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
        if (newPhase == GamePhase.PlayerTurn || newPhase == GamePhase.OpponentTurn)
        {
            bool playerWin = CheckPlayerWin();
            if (playerWin)
            {
                SetPhase(GamePhase.GameOverWin);
                return;
            }

            bool aiWin = CheckAIWin();
            if (aiWin)
            {
                SetPhase(GamePhase.GameOverLost);
                return;
            }
        }

        CurrentPhase = newPhase;

        UIGameController.Instance.SetGamePhaseText(newPhase);

        switch (newPhase)
        {
            case GamePhase.OpponentPlacement:
                PlayerUI.Instance.RefreshButtons();
                AIController.Instance.StartPlacement();
                break;
            case GamePhase.OpponentTurn:
                PlayerUI.Instance.RefreshButtons();
                AIController.Instance.Play();
                break;
            case GamePhase.PlayerTurn:
                PlayerController.Instance.CanMove = true;
                PlayerUI.Instance.RefreshButtons();
                break;
            default:
                PlayerUI.Instance.RefreshButtons();
                break;
        }
    }

    public void PlayerFinishedMoves()
    {
        if(CurrentPhase  == GamePhase.PlayerPlacement)
        {
            RevealAIPieces();
        }

        SetPhase(GamePhase.OpponentTurn);
    }

    private void RevealAIPieces()
    {
        foreach (var piece in AIController.Instance.PlacedPieces)
        {
            piece.SetVisible(true);
            piece.currentTile.SetOccupiedMarker(false);
        }
    }

    private bool CheckPlayerWin()
    {
        if (PlayerController.Instance.TotalCoins >= AIController.Instance.TotalCoins + phaseSO.pointsAdvantageToWin)
        {
            return true;
        }
        else if (!AIController.Instance.CheckCanDoAnything())
        {
            return true;
        }

        return false;
    }

    private bool CheckAIWin()
    {
        if (AIController.Instance.TotalCoins >= PlayerController.Instance.TotalCoins + phaseSO.pointsAdvantageToWin)
        {
            return true;
        }
        else if (!PlayerController.Instance.CheckCanDoAnything())
        {
            return true;
        }

        return false;
    }
}
