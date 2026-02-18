using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    public PieceDefinitionSO Definition { get; private set; }

    public void Setup(PieceDefinitionSO def)
    {
        Definition = def;
        nameText.text = def.namePt;
        costText.text = def.cost + " moedas";
        button.interactable = false;

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        HumanPlayerController.Instance.SelectPiecePlacement(Definition);
    }

    public void SetInteractable(bool interactable)
    {
        if (button.interactable == interactable) return;

        button.interactable = interactable;
    }
}
