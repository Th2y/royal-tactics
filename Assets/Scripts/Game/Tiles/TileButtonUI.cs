using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Setup(TileName tileName, bool interactable, UnityAction onClick)
    {
        nameText.text = tileName.ToString();

        SetInteractable(interactable);
        button.onClick.AddListener(onClick);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
