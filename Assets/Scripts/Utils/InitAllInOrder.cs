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

    // ========= INIT GAME =========
    GameStateController = 20,
    PlayerController = 21,
    PlayerUI = 22,
    AIController = 23,
}

public class InitAllInOrder : MonoBehaviour
{
    private UnityMethods[] systems;

    private void Awake()
    {
        systems = FindObjectsByType<UnityMethods>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(s => s.Priority)
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
