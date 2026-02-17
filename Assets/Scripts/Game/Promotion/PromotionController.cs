using UnityEngine;

public class PromotionController : UnityMethods
{
    [Header("Promotion Options")]
    [SerializeField] private PromotionUI promotionUI;

    private Pawn pendingPawn;

    public static PromotionController Instance { get; private set; }

    public override InitPriority Priority => InitPriority.PromotionController;

    public override void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void InitStart()
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
            PlayerUI.Instance.PlayerDoAnything?.Invoke();
        }
        else
        {
            GameStateController.Instance.SetPhase(GamePhase.PlayerTurn);
        }

        pendingPawn = null;
    }
    #endregion

    #region PLAYER
    private void OpenPromotionUI()
    {
        promotionUI.ShowAvailableList(GameStateController.Instance.PhaseSO.availablePieces);
    }

    public void OnPlayerSelected(PieceDefinitionSO def)
    {
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
            PieceDefinitionSO def = GameStateController.Instance.PhaseSO.availablePieces.Find(p => p.type == type);

            if (def != null) return def;
        }

        return GameStateController.Instance.PhaseSO.availablePieces[0].type != PieceType.Pawn ? 
            GameStateController.Instance.PhaseSO.availablePieces[0] : 
            GameStateController.Instance.PhaseSO.availablePieces[1];
    }
    #endregion
}
