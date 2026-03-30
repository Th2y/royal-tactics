using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingStateModeUI : GameModeUIBase
{
    [SerializeField] private KingStateButtonUI buttonKingStatePrefab;
    [SerializeField] private Transform buttonsKingStateParent;

    private readonly List<KingStateButtonUI> buttons = new();

    private bool playerGuessed = false;

    protected override void SetPlayParts()
    {
        base.SetPlayParts();

        gameTurnParent.SetActive(false);
        advantageParent.SetActive(false);
        placementParent.SetActive(false);
        promotionParent.SetActive(false);
        pieceParent.SetActive(false);
        tileParent.SetActive(false);
        kingStateParent.SetActive(true);
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
        var options = optionsT.Cast<KingStateDefinitionSO>().ToList();
        playerGuessed = false;

        foreach (Transform child in buttonsKingStateParent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        foreach (KingStateDefinitionSO state in options)
        {
            var btn = Instantiate(buttonKingStatePrefab, buttonsKingStateParent);
            btn.Setup(state, true, () => OnPlayerGuess(state.type));
            buttons.Add(btn);
        }
    }

    private void OnPlayerGuess(KingState guessedType)
    {
        if (playerGuessed) return;

        playerGuessed = true;

        if (ChooseGameMode.Instance.CurrentGameMode is KingStateMode gameMode)
        {
            gameMode.OnPlayerGuess(guessedType);
        }
    }
}
