using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
