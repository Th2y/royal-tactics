using UnityEngine;
using System.Collections.Generic;

public class ChooseColor : MonoBehaviour
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private ColorButton colorButtonPrefab;
    [SerializeField] private Transform playerColorsParent;
    [SerializeField] private Transform oponentColorsParent;

    [Header("3D Models")]
    [SerializeField] private ModelColorApplier playerModel;
    [SerializeField] private ModelColorApplier oponentModel;

    private Color selectedPlayerColor;
    private Color selectedOponentColor;

    private readonly List<ColorButton> playerButtons = new();
    private readonly List<ColorButton> oponentButtons = new();

    private void Awake()
    {
        InstantiateOptions();
        RefreshButtons();
    }

    private void InstantiateOptions()
    {
        selectedPlayerColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[0], true);
        selectedOponentColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[1], false);

        foreach (Color color in colorsOptions.colors)
        {
            // PLAYER
            var p = Instantiate(colorButtonPrefab, playerColorsParent);
            p.SetColor(color, colorsOptions.selectedColorBorder);
            p.OnClicked += OnPlayerColorSelected;
            playerButtons.Add(p);

            // OPONENT
            var o = Instantiate(colorButtonPrefab, oponentColorsParent);
            o.SetColor(color, colorsOptions.selectedColorBorder);
            o.OnClicked += OnOponentColorSelected;
            oponentButtons.Add(o);
        }
    }

    private void OnPlayerColorSelected(Color color)
    {
        if (color == selectedOponentColor)
            return;

        selectedPlayerColor = color;
        PlayerColorPrefs.SaveColor(color, true);

        playerModel.SetColor(color);
        RefreshButtons();
    }

    private void OnOponentColorSelected(Color color)
    {
        if (color == selectedPlayerColor)
            return;

        selectedOponentColor = color;
        PlayerColorPrefs.SaveColor(color, false);

        oponentModel.SetColor(color);
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        foreach (var b in playerButtons)
        {
            bool isSelected = b.color == selectedPlayerColor;
            b.SetSelected(isSelected);
            b.SetInteractable(b.color != selectedOponentColor);
        }

        foreach (var b in oponentButtons)
        {
            bool isSelected = b.color == selectedOponentColor;
            b.SetSelected(isSelected);
            b.SetInteractable(b.color != selectedPlayerColor);
        }
    }
}
