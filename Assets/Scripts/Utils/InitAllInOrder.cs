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
    BoardController = 11,
    BoardSelectionController = 12,
    PromotionController = 13,
    PlacementController = 14,
    PlacementUI = 15,

    // ========= INIT GAME =========
    // GameStateController MUST be initialized last.
    // It is responsible for starting the game flow (SetPhase / AI / Turns).
    GameStateController = 20,
}

public class InitAllInOrder : MonoBehaviour
{
    private UnityMethods[] systems;

    private void Awake()
    {
        systems = FindObjectsByType<UnityMethods>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(s => s.priority)
            .ToArray();

        foreach (var system in systems)
        {
            if (system != null) system.InitAwake();
        }
    }

    private void Start()
    {
        foreach (var system in systems)
        {
            if (system != null) system.InitStart();
        }
    }
}
