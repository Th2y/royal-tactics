using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuNavigationButton : MonoBehaviour
{
    [SerializeField] private MenuScreen menuTargetScreen;
    [SerializeField] private GameScreen gameTargetScreen;

    private Button button;

    private void Awake()
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
}