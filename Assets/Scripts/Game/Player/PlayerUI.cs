using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerUI : UnityMethods
{
    [SerializeField] private TextMeshProUGUI coinsLeftText;
    [SerializeField] private PieceButtonUI buttonPlacementPrefab;
    [SerializeField] private Transform buttonsPlacementParent;
    [SerializeField] private Button finishRoundBtn;

    private readonly List<PieceButtonUI> buttons = new();

    public Action PlayerDoAnything;

    public static PlayerUI Instance { get; private set; }

    public override InitPriority Priority => InitPriority.PlayerUI;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildUI();

        PlayerDoAnything += RefreshFinishBtnTrue;
        PlayerController.Instance.ShowPlacementButtons += RefreshButtons;
        finishRoundBtn.onClick.AddListener(() => GameStateController.Instance.PlayerFinishedMoves());
    }

    public override void InitStart()
    {
        
    }

    private void OnDestroy()
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.ShowPlacementButtons -= RefreshButtons;
    }

    private void BuildUI()
    {
        foreach (var def in GameStateController.Instance.PhaseSO.availablePieces)
        {
            var btn = Instantiate(buttonPlacementPrefab, buttonsPlacementParent);
            btn.Setup(def);
            buttons.Add(btn);
        }
    }

    public void RefreshButtons()
    {
        bool isPlayerTurn = 
            GameStateController.Instance.CurrentPhase == GamePhase.PlayerPlacement || 
            GameStateController.Instance.CurrentPhase == GamePhase.PlayerTurn;

        foreach (var btn in buttons)
        {
            bool canPlace = isPlayerTurn && PlayerController.Instance.CanPlace(btn.Definition);
            btn.SetInteractable(canPlace);
        }

        if (!isPlayerTurn) finishRoundBtn.interactable = false;
    }

    private void RefreshFinishBtnTrue()
    {
        finishRoundBtn.interactable = true;
    }

    public void ChangePointsLeftText(int points)
    {
        coinsLeftText.text = points + " moedas";
    }
}
