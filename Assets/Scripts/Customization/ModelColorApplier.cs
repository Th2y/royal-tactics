using UnityEngine;

public class ModelColorApplier : MonoBehaviour
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private string colorProperty = "_BaseColor";
    [SerializeField] private bool isPlayer = false;

    private MaterialPropertyBlock block;

    private void Awake()
    {
        block = new MaterialPropertyBlock();

        if(isPlayer)
            SetColor(PlayerColorPrefs.LoadColor(colorsOptions.colors[0], true));
        else
            SetColor(PlayerColorPrefs.LoadColor(colorsOptions.colors[1], false));
    }

    public void SetColor(Color color)
    {
        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetColor(colorProperty, color);
            r.SetPropertyBlock(block);
        }
    }
}
