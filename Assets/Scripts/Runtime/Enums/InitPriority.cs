public enum InitPriority
{
    // ========= MENU =========
    ButtonChangeSceneOrExit = 1,
    CanvasGroup = 2,
    ButtonNavigation = 3,
    ChooseColor = 4,
    ModelColorApplier = 5,

    // ========= CORE GAME INFRA =========
    ChooseGameMode = 10,
    GameModeUI = 11,
    Services = 12,
    ChooseGameModeUI = 13,
    PhaseController = 14,
    BoardController = 15,
    BoardSelectionController = 16,
    PromotionController = 17,

    // ========= INIT GAME =========
    PlayerController = 21,
    HumanPlayerUI = 22,
}