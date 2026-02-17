using System.Collections.Generic;
using UnityEngine;

public enum MenuScreen
{
    Init,
    Options,
    Tutorial
}

public class MenuController : UnityMethods
{
    [Header("Change Screen")]
    [SerializeField] private List<CanvasGroupController> screens;

    private Dictionary<MenuScreen, CanvasGroupController> _screenMap;

    public static MenuController Instance;

    public override InitPriority priority => InitPriority.UIController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetScreens();
    }

    public override void InitStart()
    {

    }

    private void SetScreens()
    {
        _screenMap = new Dictionary<MenuScreen, CanvasGroupController>();

        foreach (var screen in screens)
        {
            _screenMap.Add(screen.MenuScreen, screen);
            screen.SetActive(screen.MenuScreen == MenuScreen.Init);
        }
    }

    public void ShowScreen(MenuScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            view.SetActive(view.MenuScreen == screen);
        }
    }
}
