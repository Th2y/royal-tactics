using System.Collections.Generic;
using UnityEngine;

public enum MenuScreen
{
    Init,
    Options,
    Tutorial
}

public class MenuController : UnityMethodsSingleton<MenuController>
{
    [Header("Change Screen")]
    [SerializeField] private List<CanvasGroupController> screens;

    private Dictionary<MenuScreen, CanvasGroupController> _screenMap;

    public override InitPriority Priority => InitPriority.UIController;

    public override void OnInitAwake()
    {
        SetScreens();
    }

    public override void OnInitStart()
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
