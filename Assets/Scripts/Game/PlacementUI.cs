using UnityEngine;
using System.Collections.Generic;

public class PlacementUI : UnityMethods
{
    [SerializeField] private PieceButtonUI buttonPrefab;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private PhaseSO phaseSO;

    private readonly List<PieceButtonUI> buttons = new();

    public static PlacementUI Instance { get; private set; }

    public override InitPriority priority => InitPriority.PlacementUI;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildUI();

        PlacementController.Instance.OnPointsChanged += RefreshButtons;
    }

    public override void InitStart()
    {
        
    }

    private void OnDestroy()
    {
        if (PlacementController.Instance != null)
            PlacementController.Instance.OnPointsChanged -= RefreshButtons;
    }

    private void BuildUI()
    {
        foreach (var def in phaseSO.availablePieces)
        {
            var btn = Instantiate(buttonPrefab, buttonsParent);
            btn.Setup(def);
            buttons.Add(btn);
        }
    }

    public void RefreshButtons()
    {
        foreach (var btn in buttons)
        {
            bool canPlace = PlacementController.Instance.CanPlace(btn.definition);
            btn.SetInteractable(canPlace);
        }
    }
}
