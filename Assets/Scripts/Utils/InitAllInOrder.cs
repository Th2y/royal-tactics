using System.Linq;
using UnityEngine;

public enum InitPriority
{
    // ========= MENU / BASE UI =========
    ButtonChangeSceneOrExit = 1,
    CanvasGroup = 2,
    ButtonNavigation = 3,
    ChooseColor = 4,
    ModelColorApplier = 5,
    UIController = 6,

    // ========= CORE GAME INFRA =========
    PhaseController = 11,
    BoardController = 12,
    BoardSelectionController = 13,
    PromotionController = 14,

    // ========= INIT GAME =========
    GameStateController = 20,
    PlayerController = 21,
    PlayerUI = 22,
    AIController = 23,
}

public class InitAllInOrder : MonoBehaviour
{
    private UnityMethods[] systems;
    private IUnityMethods[] systemsS;

    private void Awake()
    {
        systemsS = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None)
            .OfType<IUnityMethods>()
            .OrderBy(s => s.Priority)
            .ToArray();

        foreach (var system in systemsS)
        {
            system?.InitAwake();
        }

        systems = FindObjectsByType<UnityMethods>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(s => s.Priority)
            .ToArray();

        foreach (var system in systems)
        {
            if (system != null) system.OnInitAwake();
        }
    }

    private void Start()
    {
        foreach (var system in systemsS)
        {
            system?.InitStart();
        }

        foreach (var system in systems)
        {
            if (system != null) system.OnInitStart();
        }
    }
}
