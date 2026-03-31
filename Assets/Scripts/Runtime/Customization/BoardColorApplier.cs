using UnityEngine;

public class BoardColorApplier : UnityMethodsSingleton<BoardColorApplier>
{
    [SerializeField] private ColorsOptions colorsOptions;
    [SerializeField] private Renderer[] renderersColor1;
    [SerializeField] private Renderer[] renderersColor2;
    [SerializeField] private Renderer rendererBoardBorderColor;
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
        Color player1Color = PlayerColorPrefs.LoadColor(colorsOptions.piecesColors[0], ColorType.Player1);
        Color player2Color = PlayerColorPrefs.LoadColor(colorsOptions.piecesColors[1], ColorType.Player2);
        Color boardBorderColor = PlayerColorPrefs.LoadColor(colorsOptions.boardBorderColors[0], ColorType.BoardBorder);

        ApplyColorToSlot(renderersColor1, player1Color);
        ApplyColorToSlot(renderersColor2, player2Color);
        ApplyColorToSlot(rendererBoardBorderColor, boardBorderColor);
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
