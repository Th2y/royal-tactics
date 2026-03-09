using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KingStateButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;

    public KingStateDefinitionSO Definition { get; private set; }

    public void Setup(KingStateDefinitionSO def, bool interactable, UnityAction onClick)
    {
        Definition = def;
        nameText.text = def.namePt;

        SetInteractable(interactable);
        button.onClick.AddListener(onClick);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
