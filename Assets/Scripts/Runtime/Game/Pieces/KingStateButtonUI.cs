using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class KingStateButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LocalizeStringEvent nameLocale;

    public KingStateDefinitionSO Definition { get; private set; }

    public void Setup(KingStateDefinitionSO def, bool interactable, UnityAction onClick)
    {
        Definition = def;
        nameLocale.StringReference = def.nameLocale;

        SetInteractable(interactable);
        button.onClick.AddListener(onClick);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
