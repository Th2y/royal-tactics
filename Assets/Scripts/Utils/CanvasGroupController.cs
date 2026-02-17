using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour
{
    [SerializeField] private MenuScreen screenType;
    [SerializeField] private GameScreen gameScreen;

    public MenuScreen ScreenType => screenType;
    public GameScreen GameScreen => gameScreen;

    private CanvasGroup cg;

    private void Awake()
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();
    }

    public void SetActive(bool active)
    {
        cg.alpha = active ? 1 : 0;
        cg.interactable = active;
        cg.blocksRaycasts = active;
    }
}
