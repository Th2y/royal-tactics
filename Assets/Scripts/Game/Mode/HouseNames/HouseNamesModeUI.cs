using System.Collections.Generic;
using UnityEngine;

public class HouseNamesModeUI : GameModeUIBase
{
    [SerializeField] private TileButtonUI buttonTilePrefab;
    [SerializeField] private Transform buttonsTilesParent;

    private readonly List<TileButtonUI> buttons = new();

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
        pieceParent.SetActive(false);
        tileParent.SetActive(true);
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
        
    }

    public override void SetOptions(List<TileName> options)
    {
        playerGuessed = false;

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();

        foreach (TileName tile in options)
        {
            var btn = Instantiate(buttonTilePrefab, buttonsTilesParent);
            btn.Setup(tile, true, () => OnPlayerGuess(tile));
            buttons.Add(btn);
        }
    }

    private void OnPlayerGuess(TileName guessedType)
    {
        if (playerGuessed) return;

        playerGuessed = true;

        if (ChooseGameMode.Instance.CurrentGameMode is HouseNamesMode gameMode)
        {
            gameMode.OnPlayerGuess(guessedType);
        }
    }
}
