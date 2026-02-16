using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromotionPieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;

    public PieceDefinitionSO definition { get; private set; }

    public void Setup(PieceDefinitionSO def)
    {
        definition = def;
        nameText.text = def.namePt;
        button.interactable = false;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PromotionController.Instance.OnPlayerSelected(definition);
    }
}
