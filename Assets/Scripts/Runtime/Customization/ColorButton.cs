using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ColorButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image borderSelected;

    public Button button;

    public Color color { get; private set; }
    private Color selectedBorderColor;

    public event Action<Color> OnClicked;

    public void SetColor(Color color, Color borderSelected)
    {
        if (button == null) button = GetComponent<Button>();

        this.color = color;
        image.color = color;
        selectedBorderColor = borderSelected;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClicked?.Invoke(this.color));
    }

    public void SetSelected(bool isSelected)
    {
        borderSelected.color = isSelected ? selectedBorderColor : color;
    }

    public void SetInteractable(bool interactable)
    {
        button.gameObject.SetActive(interactable);        
    }
}
