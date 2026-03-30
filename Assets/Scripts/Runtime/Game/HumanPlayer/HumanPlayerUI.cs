using UnityEngine;
using UnityEngine.UI;
using System;

public class HumanPlayerUI : UnityMethodsSingleton<HumanPlayerUI>
{
    [SerializeField] private Button finishBtn;

    public Action PlayerDoAnything;

    public override InitPriority Priority => InitPriority.HumanPlayerUI;

    public override void OnInitAwake()
    {
        PlayerDoAnything += RefreshFinishBtnTrue;
        finishBtn.onClick.AddListener(() => ChooseGameMode.Instance.CurrentGameMode.PlayerFinishedMoves());
    }

    public override void OnInitStart()
    {
        
    }

    private void OnDestroy()
    {
        PlayerDoAnything -= RefreshFinishBtnTrue;
    }

    public void RefreshFinishBtn()
    {
        var currentPhase = ChooseGameMode.Instance.CurrentGameMode.CurrentPhase;
        var player = HumanPlayerController.Instance;

        finishBtn.interactable =
            !player.IsInPromotion &&
            (currentPhase == GameTurn.PlayerTurn && !player.CanMove) ||
            (currentPhase == GameTurn.PlayerPlacement && player.CurrentCoins <= 1);
    }

    private void RefreshFinishBtnTrue()
    {
        finishBtn.interactable = true;
    }
}
