using UnityEngine;
using System.Collections.Generic;

public class ChooseColor : UnityMethods
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private ColorButton colorButtonPrefab;
    [SerializeField] private Transform player1ColorsParent;
    [SerializeField] private Transform player2ColorsParent;
    [SerializeField] private Transform boardBordColorsParent;

    [Header("3D Models")]
    [SerializeField] private ModelColorApplier player1Model;
    [SerializeField] private ModelColorApplier player2Model;

    private Color selectedPlayer1Color;
    private Color selectedPlayer2Color;
    private Color selectedBoardBorderColor;

    private readonly List<ColorButton> player1Buttons = new();
    private readonly List<ColorButton> player2Buttons = new();
    private readonly List<ColorButton> boardBorderButtons = new();

    public override InitPriority Priority => InitPriority.ChooseColor;

    public override void OnInitAwake()
    {
        InstantiateOptions();
        RefreshPlayersButtons();
        RefreshBoardBorderButtons();
    }

    public override void OnInitStart()
    {

    }

    private void InstantiateOptions()
    {
        selectedPlayer1Color = PlayerColorPrefs.LoadColor(colorsOptions.piecesColors[colorsOptions.defaultColorIndexPlayer1], ColorType.Player1);
        selectedPlayer2Color = PlayerColorPrefs.LoadColor(colorsOptions.piecesColors[colorsOptions.defaultColorIndexPlayer2], ColorType.Player2);
        selectedBoardBorderColor = PlayerColorPrefs.LoadColor(colorsOptions.boardBorderColors[0], ColorType.BoardBorder);

        foreach (Color color in colorsOptions.piecesColors)
        {
            // PLAYER 1
            var p = Instantiate(colorButtonPrefab, player1ColorsParent);
            p.SetColor(color, colorsOptions.onSelectedPieceColor);
            p.OnClicked += OnPlayer1ColorSelected;
            player1Buttons.Add(p);

            // PLAYER 2
            var o = Instantiate(colorButtonPrefab, player2ColorsParent);
            o.SetColor(color, colorsOptions.onSelectedPieceColor);
            o.OnClicked += OnPlayer2ColorSelected;
            player2Buttons.Add(o);
        }


        foreach(Color color in colorsOptions.boardBorderColors)
        {
            var b = Instantiate(colorButtonPrefab, boardBordColorsParent);
            b.SetColor(color, colorsOptions.onSelectedPieceColor);
            b.OnClicked += OnBoardBorderColorSelected;
            boardBorderButtons.Add(b);
        }
    }

    #region On Color Selected
    private void OnPlayer1ColorSelected(Color color)
    {
        if (color == selectedBoardBorderColor)
            return;

        selectedPlayer1Color = color;
        PlayerColorPrefs.SaveColor(color, ColorType.Player1);

        player1Model.SetColor(color);
        RefreshPlayersButtons();
    }

    private void OnPlayer2ColorSelected(Color color)
    {
        if (color == selectedPlayer1Color)
            return;

        selectedBoardBorderColor = color;
        PlayerColorPrefs.SaveColor(color, ColorType.Player2);

        player2Model.SetColor(color);
        RefreshPlayersButtons();
    }

    private void OnBoardBorderColorSelected(Color color)
    {
        if (color == selectedBoardBorderColor)
            return;

        selectedBoardBorderColor = color;
        PlayerColorPrefs.SaveColor(color, ColorType.BoardBorder);

        RefreshBoardBorderButtons();
    }
    #endregion

    #region Refresh Buttons
    private void RefreshPlayersButtons()
    {
        foreach (var b in player1Buttons)
        {
            bool isSelected = b.color == selectedPlayer1Color;
            b.SetSelected(isSelected);
            b.SetInteractable(b.color != selectedPlayer2Color);
        }

        foreach (var b in player2Buttons)
        {
            bool isSelected = b.color == selectedPlayer2Color;
            b.SetSelected(isSelected);
            b.SetInteractable(b.color != selectedPlayer1Color);
        }
    }

    private void RefreshBoardBorderButtons()
    {
        foreach (var b in boardBorderButtons)
        {
            bool isSelected = b.color == selectedBoardBorderColor;
            b.SetSelected(isSelected);
            b.SetInteractable(true);
        }
    }
    #endregion
}
