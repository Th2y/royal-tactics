using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneButton : UnityMethods
{
    [SerializeField] private string sceneName;

    private Button button;

    public override InitPriority Priority => InitPriority.ButtonChangeSceneOrExit;

    public override void InitAwake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    public override void InitStart() { }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}