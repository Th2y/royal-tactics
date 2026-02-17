using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : UnityMethods
{
    [SerializeField] private MenuScreen menuScreen;
    [SerializeField] private GameScreen gameScreen;

    public override InitPriority priority => InitPriority.CanvasGroup;

    public MenuScreen MenuScreen => menuScreen;
    public GameScreen GameScreen => gameScreen;

    private CanvasGroup cg;

    public override void InitAwake()
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();
    }

    public override void InitStart() { }

    public void SetActive(bool active)
    {
        cg.alpha = active ? 1 : 0;
        cg.interactable = active;
        cg.blocksRaycasts = active;
    }
}
