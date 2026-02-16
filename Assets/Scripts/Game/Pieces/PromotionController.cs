using UnityEngine;
using System.Collections.Generic;

public class PromotionController : MonoBehaviour
{
    [Header("Promotion Options")]
    [SerializeField] private PhaseSO phaseSO;
    [SerializeField] private PromotionUI promotionUI;

    private Pawn pendingPawn;

    public static PromotionController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

        pendingPawn = null;
    }
    #endregion

    #region PLAYER
    private void OpenPromotionUI()
    {
        promotionUI.ShowAvailableList(phaseSO.availablePieces);
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
        PieceDefinitionSO best = phaseSO.availablePieces[Random.Range(0, phaseSO.availablePieces.Count - 1)];
        return best;
    }
    #endregion
}
