using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        promotionParent.SetActive(false);
        finishBtn.gameObject.SetActive(true);
    }

    #region Total Points
    [Header("Total Points")]
    [SerializeField] private TextMeshProUGUI advantagePointsText;
    [SerializeField] private TextMeshProUGUI playerTotalPointsText;
    [SerializeField] private TextMeshProUGUI aiTotalPointsText;

    [HideInInspector]
    private void SetAdvantagePoints(int points)
    {
        advantagePointsText.text = "Vantagem necessária: " + points;
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

    [SerializeField] private TextMeshProUGUI coinsLeftText;
    [SerializeField] private PieceButtonUI buttonPlacementPrefab;
    [SerializeField] private Transform buttonsPlacementParent;

    private readonly List<PieceButtonUI> buttons = new();

    public override void BuildUI()
    {
        SetOptions(PhaseController.Instance.CurrentPhase.availablePiecesHuman);
    }

    public override void SetOptions(List<PieceDefinitionSO> options)
    {
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

    public override void SetOptions(List<TileName> options)
    {

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

    private void ChangeCoinsLeftText(int coins)
    {
        coinsLeftText.text = coins + " moedas";
    }
}
