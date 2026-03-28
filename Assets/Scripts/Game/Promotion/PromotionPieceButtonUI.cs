using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PromotionPieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LocalizeStringEvent nameLocale;

    public PieceDefinitionSO Definition { get; private set; }

    public void Setup(PieceDefinitionSO def)
    {
        Definition = def;
        nameLocale.StringReference = def.nameT;
        button.interactable = true;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PromotionController.Instance.OnPlayerSelected(Definition);
    }
}
