using UnityEngine;

public class BoardColorApplier : UnityMethodsSingleton<BoardColorApplier>
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private Renderer[] renderersColor1;
    [SerializeField] private Renderer[] renderersColor2;
    [SerializeField] private string colorProperty = "_BaseColor";

    private MaterialPropertyBlock block;

    public override InitPriority Priority => InitPriority.ModelColorApplier;

    public override void OnInitAwake()
    {
        block = new MaterialPropertyBlock();
        ApplySavedColors();
    }

    public override void OnInitStart()
    {

    }

    private void ApplySavedColors()
    {
        Color playerColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[0], true);
        Color opponentColor = PlayerColorPrefs.LoadColor(colorsOptions.colors[1], false);

        ApplyColorToSlot(renderersColor1, playerColor);
        ApplyColorToSlot(renderersColor2, opponentColor);
    }

    private void ApplyColorToSlot(Renderer[] renderers, Color color)
    {
        foreach (var r in renderers)
        {
            r.GetPropertyBlock(block);
            block.SetColor(colorProperty, color);
            r.SetPropertyBlock(block);

            r.GetComponent<Tile>().Init(color);
        }
    }

    public void ApplyColorToSlot(Renderer renderer, Color color)
    {
        renderer.GetPropertyBlock(block);
        block.SetColor(colorProperty, color);
        renderer.SetPropertyBlock(block);
    }
}
