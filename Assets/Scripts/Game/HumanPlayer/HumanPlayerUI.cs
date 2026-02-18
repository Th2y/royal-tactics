using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class HumanPlayerUI : UnityMethodsSingleton<HumanPlayerUI>
{
    [SerializeField] private TextMeshProUGUI coinsLeftText;
    [SerializeField] private PieceButtonUI buttonPlacementPrefab;
    [SerializeField] private Transform buttonsPlacementParent;
    [SerializeField] private Button finishRoundBtn;

    private readonly List<PieceButtonUI> buttons = new();

    public Action PlayerDoAnything;

    public override InitPriority Priority => InitPriority.PlayerUI;

    public override void OnInitAwake()
    {
        BuildUI();

        PlayerDoAnything += RefreshFinishBtnTrue;
        HumanPlayerController.Instance.ShowPlacementButtons += RefreshButtons;
        HumanPlayerController.Instance.OnCoinsChanged += ChangeCoinsLeftText;
        finishRoundBtn.onClick.AddListener(() => GameStateController.Instance.PlayerFinishedMoves());
    }

    public override void OnInitStart()
    {
        ChangeCoinsLeftText(HumanPlayerController.Instance.CurrentCoins);
    }

    private void OnDestroy()
    {
        PlayerDoAnything -= RefreshFinishBtnTrue;

        if (HumanPlayerController.Instance != null)
        {
            HumanPlayerController.Instance.ShowPlacementButtons -= RefreshButtons;
            HumanPlayerController.Instance.OnCoinsChanged -= ChangeCoinsLeftText;
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
        var player = HumanPlayerController.Instance;

        bool isPlayerTurn = 
            currentPhase == GamePhase.PlayerPlacement || 
            (currentPhase == GamePhase.PlayerTurn && player.CanMove);

        foreach (var btn in buttons)
        {
            bool canPlace = isPlayerTurn && player.HaveEnoughCoinsToPlace(btn.Definition);
            btn.SetInteractable(canPlace);
        }

        finishRoundBtn.interactable = 
            !player.IsInPromotion &&
            (currentPhase == GamePhase.PlayerTurn && !player.CanMove) ||
            (currentPhase == GamePhase.PlayerPlacement && player.CurrentCoins <= 1);
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
