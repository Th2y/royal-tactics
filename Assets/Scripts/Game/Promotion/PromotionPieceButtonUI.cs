using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromotionPieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;

    public PieceDefinitionSO Definition { get; private set; }

    public void Setup(PieceDefinitionSO def)
    {
        Definition = def;
        nameText.text = def.namePt;
        button.interactable = true;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PromotionController.Instance.OnPlayerSelected(Definition);
    }
}
