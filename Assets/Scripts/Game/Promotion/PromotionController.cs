using UnityEngine;

public class PromotionController : UnityMethodsSingleton<PromotionController>
{
    [Header("Promotion Options")]
    [SerializeField] private PromotionUI promotionUI;

    private Pawn pendingPawn;

    public override InitPriority Priority => InitPriority.PromotionController;

    public override void OnInitAwake()
    {

    }

    public override void OnInitStart()
    {

    }

    #region CORE LOGIC
    public void RequestPromotion(Pawn pawn)
    {
        if (pawn == null || !pawn.CanPromote) return;

        pendingPawn = pawn;

        if (pawn.isFromPlayer)
        {
            OpenPromotionUI();
        }
        else
        {
            Promote(pawn, ChooseAIPromotion());
        }
    }

    private void Promote(Pawn pawn, PieceDefinitionSO newDefinition)
    {
        Tile tile = pawn.currentTile;
        bool isFromPlayer = pawn.isFromPlayer;

        tile.Clear();
        Destroy(pawn.gameObject);

        Piece newPiece = Instantiate(newDefinition.prefab);
        newPiece.Initialize(newDefinition, isFromPlayer);
        tile.SetPiece(newPiece);

        if (isFromPlayer)
        {
            HumanPlayerController.Instance.OnPromote(pawn, newPiece);
            HumanPlayerUI.Instance.PlayerDoAnything?.Invoke();
        }
        else
        {
            AIController.Instance.OnPromote(pawn, newPiece);
            GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
        }

        pendingPawn = null;
    }
    #endregion

    #region PLAYER
    private void OpenPromotionUI()
    {
        HumanPlayerController.Instance.IsInPromotion = true;
        HumanPlayerUI.Instance.RefreshButtons();

        promotionUI.ShowAvailableList(PhaseController.Instance.CurrentPhase.availablePieces);
    }

    public void OnPlayerSelected(PieceDefinitionSO def)
    {
        HumanPlayerController.Instance.IsInPromotion = false;
        HumanPlayerUI.Instance.RefreshButtons();

        promotionUI.HideAvailableList();
        Promote(pendingPawn, def);
    }
    #endregion

    #region IA
    private PieceDefinitionSO ChooseAIPromotion()
    {
        PieceType[] priority =
        {
            PieceType.Queen,
            PieceType.Rook,
            PieceType.Bishop,
            PieceType.Knight
        };

        foreach (PieceType type in priority)
        {
            PieceDefinitionSO def = PhaseController.Instance.CurrentPhase.availablePieces.Find(p => p.type == type);

            if (def != null) return def;
        }

        return PhaseController.Instance.CurrentPhase.availablePieces[0].type != PieceType.Pawn ? 
            PhaseController.Instance.CurrentPhase.availablePieces[0] : 
            PhaseController.Instance.CurrentPhase.availablePieces[1];
    }
    #endregion
}
