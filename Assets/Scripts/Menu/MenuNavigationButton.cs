using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuNavigationButton : MonoBehaviour
{
    [SerializeField] private MenuController menuController;
    [SerializeField] private MenuScreen targetScreen;

    private Button button;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (button == null) button = GetComponent<Button>();
    }
#endif

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();

        button.onClick.AddListener(() => menuController.ShowScreen(targetScreen));
    }
}