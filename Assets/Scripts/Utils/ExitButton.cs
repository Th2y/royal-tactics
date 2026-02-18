using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : UnityMethods
{
    private Button button;

    public override InitPriority Priority => InitPriority.ButtonChangeSceneOrExit;

    public override void OnInitAwake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(ExitGame);
    }

    public override void OnInitStart(){}

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
