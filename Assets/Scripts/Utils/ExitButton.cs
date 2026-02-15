using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
}
