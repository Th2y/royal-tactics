using System.Collections.Generic;
using UnityEngine;

public class MysteryPieceModeUI : GameModeUIBase
{
    [SerializeField] private PieceButtonUI buttonPiecePrefab;
    [SerializeField] private Transform buttonsPiecesParent;

    private readonly List<PieceButtonUI> buttons = new();

    private bool playerGuessed = false;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void SetPlayParts()
    {
        base.SetPlayParts();

        gameTurnParent.SetActive(false);
        advantageParent.SetActive(false);
        placementParent.SetActive(false);
        promotionParent.SetActive(false);
        pieceParent.SetActive(true);
        finishBtn.gameObject.SetActive(false);
    }

    public override void BuildUI()
    {

    }

    public override void RefreshButtons()
    {

    }

    public override void SetOptions(List<PieceDefinitionSO> options)
    {
        playerGuessed = false;

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();

        foreach (PieceDefinitionSO piece in options)
        {
            var btn = Instantiate(buttonPiecePrefab, buttonsPiecesParent);
            btn.Setup(piece, false, true, () => OnPlayerGuess(piece.type));
            buttons.Add(btn);
        }
    }

    private void OnPlayerGuess(PieceType guessedType)
    {
        if (playerGuessed) return;

        playerGuessed = true;

        if (ChooseGameMode.Instance.CurrentGameMode is MysteryPieceMode gameMode)
        {
            gameMode.OnPlayerGuess(guessedType);
        }
    }
}
