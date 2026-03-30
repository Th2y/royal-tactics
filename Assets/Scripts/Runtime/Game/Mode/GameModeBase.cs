using UnityEngine;

public abstract class GameModeBase : MonoBehaviour
{
    public GameModeSO GameModeSO;

    public GameTurn CurrentPhase { get; protected set; }

    public bool IsPlayerTurn =>
        CurrentPhase == GameTurn.PlayerTurn ||
        CurrentPhase == GameTurn.PlayerPlacement;

    public abstract void SetGameTurn(GameTurn newPhase);

    public virtual void StartMode()
    {
        ChooseGameModeUI.Instance.CurrentGameMode.BuildUI();
        ChooseGameModeUI.Instance.CurrentGameMode.SetInitialValues();
        BoardController.Instance.ClearBoard();
        HumanPlayerController.Instance.ResetGame();
        AIController.Instance.ResetGame();
        BoardController.Instance.HandleColliderAllTiles(true);
    }

    public abstract void PlayerFinishedMoves();

    protected void RevealAIPieces()
    {
        foreach (var piece in AIController.Instance.PlacedPieces)
        {
            piece.SetVisible(true);
            piece.CurrentTile.SetOccupiedMarker(false);
        }
    }

    protected abstract bool CheckPlayerWin();

    protected abstract bool CheckAIWin();
}
