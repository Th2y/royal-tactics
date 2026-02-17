using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameScreen
{
    Play,
    Finish,
    Pause
}

public class UIGameController : MonoBehaviour
{
    public static UIGameController Instance;

    private void Awake()
    {
        Instance = this;

        SetScreens();
    }

    #region Game Screen
    [Header("Change Screen")]
    [SerializeField] private List<CanvasGroupController> screens;

    private Dictionary<GameScreen, CanvasGroupController> _screenMap;

    private void SetScreens()
    {
        _screenMap = new Dictionary<GameScreen, CanvasGroupController>();

        foreach (var screen in screens)
        {
            _screenMap.Add(screen.GameScreen, screen);
            screen.SetActive(screen.GameScreen == GameScreen.Play);
        }
    }

    public void ShowScreen(GameScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            view.SetActive(view.GameScreen == screen);
        }
    }
    #endregion

    #region Phrase by Game Phase
    [Header("Game Phase")]
    [SerializeField] private TextMeshProUGUI gamePhaseText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private string timeToPlayerPlay = "É a sua vez de jogar";
    [SerializeField] private string timeToAIPlay = "É a vez do oponente jogar";
    [SerializeField] private string timeToPlayerPlacement = "É a sua vez de colocar as peças";
    [SerializeField] private string timeToAIPlacement = "É a vez do oponente colocar as peças";
    [SerializeField] private string playerWin = "Você venceu!";
    [SerializeField] private string playerLost = "Você perdeu!";

    public void SetGamePhaseText(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.OpponentPlacement:
                gamePhaseText.text = timeToAIPlacement;
                break;
            case GamePhase.PlayerPlacement:
                gamePhaseText.text = timeToPlayerPlacement;
                break;
            case GamePhase.OpponentTurn:
                gamePhaseText.text = timeToAIPlay;
                break;
            case GamePhase.PlayerTurn:
                gamePhaseText.text = timeToPlayerPlay;
                break;
            case GamePhase.GameOverWin:
                gameOverText.text = playerWin;
                ShowScreen(GameScreen.Finish);
                break;
            case GamePhase.GameOverLost:
                gameOverText.text = playerLost;
                ShowScreen(GameScreen.Finish);
                break;
        }
    }
    #endregion
}
