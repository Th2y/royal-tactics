using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class PointAdvantageModeUI : GameModeUIBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        HumanPlayerController.Instance.OnTotalCoinsChanged += SetPlayerTotalPoints;
        AIController.Instance.OnTotalCoinsChanged += SetAITotalPoints;
        HumanPlayerController.Instance.ShowPlacementButtons += RefreshButtons;
        HumanPlayerController.Instance.OnCoinsChanged += ChangeCoinsLeftText;

        ChangeCoinsLeftText(HumanPlayerController.Instance.CurrentCoins);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HumanPlayerController.Instance.OnTotalCoinsChanged -= SetPlayerTotalPoints;
        AIController.Instance.OnTotalCoinsChanged -= SetAITotalPoints;
        HumanPlayerController.Instance.ShowPlacementButtons -= RefreshButtons;
        HumanPlayerController.Instance.OnCoinsChanged -= ChangeCoinsLeftText;
    }

    public override void SetInitialValues()
    {
        SetAdvantagePoints(PhaseController.Instance.CurrentPhase.pointsAdvantageToWin);
        SetPlayerTotalPoints(HumanPlayerController.Instance.TotalCoins);
        SetAITotalPoints(AIController.Instance.TotalCoins);
    }

    protected override void SetPlayParts()
    {
        base.SetPlayParts();
        gameTurnParent.SetActive(true);
        advantageParent.SetActive(true);
        placementParent.SetActive(true);
        pieceParent.SetActive(false);
        tileParent.SetActive(false);
        kingStateParent.SetActive(false);
        promotionParent.SetActive(false);
        finishBtn.gameObject.SetActive(true);
    }

    #region Total Points
    [Header("Total Points")]
    [SerializeField] private LocalizeStringEvent advantagePointsLocale;
    [SerializeField] private LocalizeStringEvent playerTotalPointsLocale;
    [SerializeField] private LocalizeStringEvent opponentTotalPointsLocale;

    [HideInInspector]
    private void SetAdvantagePoints(int points)
    {
        var coinsCount = advantagePointsLocale.StringReference["value"] as IntVariable;
        coinsCount.Value = points;
    }

    [HideInInspector]
    public void SetPlayerTotalPoints(int totalPoints)
    {
        var coinsCount = playerTotalPointsLocale.StringReference["value"] as IntVariable;
        coinsCount.Value = totalPoints;
    }

    [HideInInspector]
    public void SetAITotalPoints(int totalPoints)
    {
        var coinsCount = opponentTotalPointsLocale.StringReference["value"] as IntVariable;
        coinsCount.Value = totalPoints;
    }
    #endregion

    [SerializeField] private LocalizeStringEvent coinsLeftLocale;
    [SerializeField] private PieceButtonUI buttonPlacementPrefab;
    [SerializeField] private Transform buttonsPlacementParent;

    private readonly List<PieceButtonUI> buttons = new();

    public override void BuildUI()
    {
        SetOptions(PhaseController.Instance.CurrentPhase.availablePiecesHuman);
    }

    public override void SetOptions<T>(List<T> optionsT)
    {
        var options = optionsT.Cast<PieceDefinitionSO>().ToList();

        foreach (Transform child in buttonsPlacementParent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        foreach (var def in options)
        {
            var btn = Instantiate(buttonPlacementPrefab, buttonsPlacementParent);
            btn.Setup(def, true, false, () => HumanPlayerController.Instance.SelectPiecePlacement(def));
            buttons.Add(btn);
        }
    }

    public override void RefreshButtons()
    {
        var currentPhase = ChooseGameMode.Instance.CurrentGameMode.CurrentPhase;
        var player = HumanPlayerController.Instance;

        bool isPlayerTurn =
            currentPhase == GameTurn.PlayerPlacement ||
            (currentPhase == GameTurn.PlayerTurn && player.CanMove);

        foreach (var btn in buttons)
        {
            bool canPlace = isPlayerTurn && player.HaveEnoughCoinsToPlace(btn.Definition);
            btn.SetInteractable(canPlace);
        }

        HumanPlayerUI.Instance.RefreshFinishBtn();
    }

    private void ChangeCoinsLeftText(int count)
    {
        var coinsCount = coinsLeftLocale.StringReference["count"] as IntVariable;
        coinsCount.Value = count;
    }
}
