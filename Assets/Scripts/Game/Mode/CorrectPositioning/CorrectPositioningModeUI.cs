using System.Collections.Generic;
using UnityEngine;

public class CorrectPositioningModeUI : GameModeUIBase
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
        playerGuessed = false;

        foreach (Transform child in buttonsTilesParent)
        {
            Destroy(child.gameObject);
        }
        buttons.Clear();

        foreach (PieceDefinitionSO piece in options)
        {
            var btn = Instantiate(buttonTilePrefab, buttonsTilesParent);
            List<TileName> tileNames = new(piece.humanInitialTiles);
            tileNames.AddRange(piece.aiInitialTiles);
            btn.Setup(tileNames, true, () => OnPlayerGuess(piece.type));
            buttons.Add(btn);
        }
    }

    public override void SetOptions(List<TileName> options)
    {
        
    }

    private void OnPlayerGuess(PieceType guessedType)
    {
        if (playerGuessed) return;

        playerGuessed = true;

        if (ChooseGameMode.Instance.CurrentGameMode is CorrectPositioningMode gameMode)
        {
            gameMode.OnPlayerGuess(guessedType);
        }
    }
}
