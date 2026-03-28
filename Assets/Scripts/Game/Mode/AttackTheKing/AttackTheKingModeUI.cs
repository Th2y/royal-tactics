using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackTheKingModeUI : GameModeUIBase
{
    [SerializeField] private PieceButtonUI buttonPiecePrefab;
    [SerializeField] private Transform buttonsPiecesParent;

    private readonly List<PieceButtonUI> buttons = new();

    private bool playerGuessed = false;

    protected override void SetPlayParts()
    {
        base.SetPlayParts();
        gameTurnParent.SetActive(false);
        advantageParent.SetActive(false);
        placementParent.SetActive(false);
        pieceParent.SetActive(true);
        tileParent.SetActive(false);
        kingStateParent.SetActive(false);
        promotionParent.SetActive(false);
        finishBtn.gameObject.SetActive(false);
    }

    public override void BuildUI()
    {
        
    }

    public override void RefreshButtons()
    {
        
    }

    public override void SetOptions<T>(List<T> optionsT)
    {
        var options = optionsT.Cast<PieceDefinitionSO>().ToList();

        playerGuessed = false;

        foreach (Transform child in buttonsPiecesParent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        foreach (PieceDefinitionSO piece in options)
        {
            var btn = Instantiate(buttonPiecePrefab, buttonsPiecesParent);
            btn.Setup(piece, false, true, () => HumanPlayerController.Instance.SelectPiecePlacement(piece));
            buttons.Add(btn);
        }
    }
}
