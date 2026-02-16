using System.Collections.Generic;
using UnityEngine;

public class PromotionUI : MonoBehaviour
{
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private PromotionPieceButtonUI buttonPrefab;
    [SerializeField] private CanvasGroupController cg;

    private readonly List<PromotionPieceButtonUI> buttons = new();

    public void ShowAvailableList(List<PieceDefinitionSO> availablePieces)
    {
        cg.SetActive(true);

        ClearButtons();

        foreach (var def in availablePieces)
        {
            var btn = Instantiate(buttonPrefab, buttonsParent);
            btn.Setup(def);
            buttons.Add(btn);
        }
    }

    public void HideAvailableList()
    {
        cg.SetActive(false);

        ClearButtons();
    }

    private void ClearButtons()
    {
        if (buttons == null || buttons.Count <= 0) return;

        foreach (var btn in buttons)
        {
            Destroy(btn.gameObject);
        }

        buttons.Clear();
    }
}
