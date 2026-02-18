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
        PlayerController.Instance.OnCoinsChanged += ChangeCoinsLeftText;
        finishRoundBtn.onClick.AddListener(() => GameStateController.Instance.PlayerFinishedMoves());
    }

    public override void InitStart()
    {
        
    }

    private void OnDestroy()
    {
        PlayerDoAnything -= RefreshFinishBtnTrue;

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.ShowPlacementButtons -= RefreshButtons;
            PlayerController.Instance.OnCoinsChanged -= ChangeCoinsLeftText;
        }
    }

    private void BuildUI()
    {
        foreach (var def in PhaseController.Instance.CurrentPhase.availablePieces)
        {
            var btn = Instantiate(buttonPlacementPrefab, buttonsPlacementParent);
            btn.Setup(def);
            buttons.Add(btn);
        }
    }

    public void RefreshButtons()
    {
        var currentPhase = GameStateController.Instance.CurrentPhase;

        bool isPlayerTurn = 
            currentPhase == GamePhase.PlayerPlacement || 
            (currentPhase == GamePhase.PlayerTurn && PlayerController.Instance.CanMove);

        foreach (var btn in buttons)
        {
            bool canPlace = isPlayerTurn && PlayerController.Instance.HaveEnoughCoinsToPlace(btn.Definition);
            btn.SetInteractable(canPlace);
        }

        finishRoundBtn.interactable = !PlayerController.Instance.CanMove && 
           currentPhase == GamePhase.PlayerPlacement || currentPhase == GamePhase.PlayerTurn;
    }

    private void RefreshFinishBtnTrue()
    {
        finishRoundBtn.interactable = true;
    }

    private void ChangeCoinsLeftText(int coins)
    {
        coinsLeftText.text = coins + " moedas";
    }
}
