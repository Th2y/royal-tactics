using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameScreen
{
    Play,
    Finish,
    Pause
}

public class UIGameController : UnityMethods
{
    public static UIGameController Instance;

    public override InitPriority Priority => InitPriority.UIController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetScreens();
    }

    public override void InitStart()
    {
        PlayerController.Instance.OnTotalCoinsChanged += SetPlayerTotalPoints;
        AIController.Instance.OnTotalCoinsChanged += SetAITotalPoints;
    }

    private void OnDestroy()
    {
        PlayerController.Instance.OnTotalCoinsChanged -= SetPlayerTotalPoints;
        AIController.Instance.OnTotalCoinsChanged -= SetAITotalPoints;
        SetAdvantagePoints(GameStateController.Instance.PhaseSO.pointsAdvantageToWin);
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

    #region Total Points
    [Header("Total Points")]
    [SerializeField] private TextMeshProUGUI advantagePointsText;
    [SerializeField] private TextMeshProUGUI playerTotalPointsText;
    [SerializeField] private TextMeshProUGUI aiTotalPointsText;

    [HideInInspector]
    private void SetAdvantagePoints(int points)
    {
        aiTotalPointsText.text = "Vantagem necessária: " + points;
    }

    [HideInInspector]
    public void SetPlayerTotalPoints(int totalPoints)
    {
        playerTotalPointsText.text = "Você: " + totalPoints;
    }

    [HideInInspector]
    public void SetAITotalPoints(int totalPoints)
    {
        aiTotalPointsText.text = "Oponente: " + totalPoints;
    }
    #endregion
}
