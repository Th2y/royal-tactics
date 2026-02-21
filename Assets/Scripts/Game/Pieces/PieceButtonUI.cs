using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    public PieceDefinitionSO Definition { get; private set; }

    public void Setup(PieceDefinitionSO def, bool showCost, bool interactable, UnityAction onClick)
    {
        Definition = def;
        nameText.text = def.namePt;

        if (showCost)
        {
            costText.gameObject.SetActive(true);
            costText.text = def.cost + " moedas";
        }
        else
        {
            costText.gameObject.SetActive(false);
        }

        SetInteractable(interactable);
        button.onClick.AddListener(onClick);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
