using UnityEngine;

public class ModelColorApplier : UnityMethods
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private string colorProperty = "_BaseColor";

    public bool isPlayer = false;

    private MaterialPropertyBlock block;

    public override InitPriority priority => InitPriority.ModelColorApplier;

    public override void InitAwake()
    {
        block = new MaterialPropertyBlock();

        if (isPlayer)
            SetColor(PlayerColorPrefs.LoadColor(colorsOptions.colors[0], true));
        else
            SetColor(PlayerColorPrefs.LoadColor(colorsOptions.colors[1], false));
    }

    public override void InitStart()
    {
        
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
