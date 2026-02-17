using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavigationButton : UnityMethods
{
    [SerializeField] private MenuScreen menuTargetScreen;
    [SerializeField] private GameScreen gameTargetScreen;

    private Button button;

    public override InitPriority priority => InitPriority.ButtonNavigation;

    public override void InitAwake()
    {
        if (button == null) button = GetComponent<Button>();

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Menu")
        {
            button.onClick.AddListener(() => MenuController.Instance.ShowScreen(menuTargetScreen));
        }
        else
        {
            button.onClick.AddListener(() => UIGameController.Instance.ShowScreen(gameTargetScreen));
        }
    }

    public override void InitStart() { }
}