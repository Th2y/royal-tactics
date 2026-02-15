using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour
{
    [SerializeField] private MenuScreen screenType;

    public MenuScreen ScreenType => screenType;
    private CanvasGroup cg;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (cg == null) cg = GetComponent<CanvasGroup>();
    }
#endif

    public void SetActive(bool active)
    {
        cg.alpha = active ? 1 : 0;
        cg.interactable = active;
        cg.blocksRaycasts = active;
    }
}
