using System.Collections.Generic;
using UnityEngine;

public enum MenuScreen
{
    Init,
    Options,
    Tutorial
}

public class MenuController : MonoBehaviour
{
    [Header("Change Screen")]
    [SerializeField] private List<CanvasGroupController> screens;

    private Dictionary<MenuScreen, CanvasGroupController> _screenMap;

    private void Awake()
    {
        SetScreens();
    }

    private void SetScreens()
    {
        _screenMap = new Dictionary<MenuScreen, CanvasGroupController>();

        foreach (var screen in screens)
        {
            _screenMap.Add(screen.ScreenType, screen);
            screen.SetActive(screen.ScreenType == MenuScreen.Init);
        }
    }

    public void ShowScreen(MenuScreen screen)
    {
        foreach (var view in _screenMap.Values)
        {
            view.SetActive(view.ScreenType == screen);
        }
    }
}
