using UnityEngine;
using System.Collections.Generic;

public class PlacementUI : MonoBehaviour
{
    [SerializeField] private PieceButtonUI buttonPrefab;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private PhaseSO phaseSO;

    private readonly List<PieceButtonUI> buttons = new();

    public static PlacementUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BuildUI();

        PlacementController.Instance.OnPointsChanged += RefreshButtons;
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
