using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private Button button;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}