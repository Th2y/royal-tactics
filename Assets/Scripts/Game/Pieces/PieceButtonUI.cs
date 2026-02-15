using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    public PieceDefinitionSO definition {  get; private set; }

    public void Setup(PieceDefinitionSO def)
    {
        definition = def;
        nameText.text = def.namePt;
        costText.text = def.cost.ToString();
        button.interactable = false;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        PlacementController.Instance.SelectPiece(definition);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
