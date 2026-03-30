using UnityEngine;

public class PromotionController : UnityMethodsSingleton<PromotionController>
{
    [Header("Promotion Options")]
    [SerializeField] private PromotionUI promotionUI;

    private Piece pendingPiece;

    public override InitPriority Priority => InitPriority.PromotionController;

    public override void OnInitAwake()
    {

    }

    public override void OnInitStart()
    {

    }

    #region CORE LOGIC
    public void RequestPromotion(Piece piece)
    {
        if (piece == null || !piece.CanPromote) return;

        pendingPiece = piece;

        if (piece.IsFromPlayer)
        {
            OpenPromotionUI();
        }
        else
        {
            Promote(piece, ChooseAIPromotion());
        }
    }

    private void Promote(Piece piece, PieceDefinitionSO newDefinition)
    {
        Tile tile = piece.CurrentTile;
        bool isFromPlayer = piece.IsFromPlayer;

        tile.Clear();
        Destroy(piece.gameObject);

        Piece newPiece = Instantiate(newDefinition.prefab);
        newPiece.Initialize(newDefinition, isFromPlayer);
        tile.SetPiece(newPiece);

        if (isFromPlayer)
        {
            HumanPlayerController.Instance.OnPromote(piece, newPiece);
            HumanPlayerUI.Instance.PlayerDoAnything?.Invoke();
        }
        else
        {
            AIController.Instance.OnPromote(piece, newPiece);
            ChooseGameMode.Instance.CurrentGameMode.SetGameTurn(GameTurn.PlayerTurn);
        }

        pendingPiece = null;
    }
    #endregion

    #region PLAYER
    private void OpenPromotionUI()
    {
        HumanPlayerController.Instance.IsInPromotion = true;
        ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();

        promotionUI.ShowAvailableList(pendingPiece.Definition.type, PhaseController.Instance.CurrentPhase.availablePiecesPromotion);
    }

    public void OnPlayerSelected(PieceDefinitionSO def)
    {
        HumanPlayerController.Instance.IsInPromotion = false;
        ChooseGameModeUI.Instance.CurrentGameMode.RefreshButtons();

        promotionUI.HideAvailableList();
        Promote(pendingPiece, def);
    }
    #endregion

    #region IA
    private PieceDefinitionSO ChooseAIPromotion()
    {
        PieceType currentType = pendingPiece.Definition.type;

        var available = PhaseController.Instance.CurrentPhase.availablePiecesPromotion;

        var validOptions = available.FindAll(p => p.type != currentType);

        if (validOptions.Count == 0)
        {
            Debug.LogError("Without available promotion pieces");
            return null;
        }

        return validOptions[Random.Range(0, validOptions.Count)];
    }
    #endregion
}
