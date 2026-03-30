using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class PieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LocalizeStringEvent nameLocale;
    [SerializeField] private LocalizeStringEvent costLocale;

    public PieceDefinitionSO Definition { get; private set; }

    public void Setup(PieceDefinitionSO def, bool showCost, bool interactable, UnityAction onClick)
    {
        Definition = def;
        nameLocale.StringReference = def.nameT;

        if (showCost)
        {
            var coinsCount = costLocale.StringReference["count"] as IntVariable;
            coinsCount.Value = def.cost;
        }
        else
        {
            costLocale.gameObject.SetActive(false);
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
