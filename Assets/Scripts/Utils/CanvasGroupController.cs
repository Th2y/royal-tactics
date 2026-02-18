using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : UnityMethods
{
    [SerializeField] private MenuScreen menuScreen;
    [SerializeField] private GameScreen gameScreen;

    public override InitPriority Priority => InitPriority.CanvasGroup;

    public MenuScreen MenuScreen => menuScreen;
    public GameScreen GameScreen => gameScreen;

    private CanvasGroup cg;

    public override void OnInitAwake()
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();
    }

    public override void OnInitStart() { }

    public void SetActive(bool active)
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();

        cg.alpha = active ? 1 : 0;
        cg.interactable = active;
        cg.blocksRaycasts = active;
    }
}
