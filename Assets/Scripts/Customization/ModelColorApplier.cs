using UnityEngine;

public class ModelColorApplier : UnityMethods
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private string colorProperty = "_BaseColor";

    public bool isPlayer = false;

    private MaterialPropertyBlock block;

    public Color DefaultColor { get; private set; }
    public Color LastColor { get; private set; }

    public override InitPriority Priority => InitPriority.ModelColorApplier;

    public override void InitAwake()
    {
        block = new MaterialPropertyBlock();

        if (isPlayer)
            DefaultColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[0], true);
        else
            DefaultColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[1], false);

        SetColor(DefaultColor);
    }

    public override void InitStart()
    {
        
    }

    public void SetColor(Color color, bool changeLastColor = true)
    {
        if (changeLastColor) LastColor = color;

        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetColor(colorProperty, color);
            r.SetPropertyBlock(block);
        }
    }
}
