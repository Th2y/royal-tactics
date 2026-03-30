using System;
using System.Collections.Generic;

public abstract class BasePlayerController<T> : UnityMethodsSingleton<T> where T : BasePlayerController<T>
{
    public override InitPriority Priority => InitPriority.PlayerController;

    public List<Piece> PlacedPieces { get; protected set; } = new();

    public virtual int CurrentCoins { get; protected set; }

    public event Action<int> OnTotalCoinsChanged;
    protected int totalCoins;
    public int TotalCoins
    {
        get => totalCoins;
        set
        {
            if (totalCoins == value)
                return;

            totalCoins = value;
            OnTotalCoinsChanged?.Invoke(totalCoins);
        }
    }

    #region Unity Default Methods
    public override void OnInitAwake()
    {

    }

    public override void OnInitStart()
    {
        
    }
    #endregion

    public void ResetGame()
    {
        PlacedPieces.Clear();
        CurrentCoins = PhaseController.Instance.CurrentPhase.startingPoints;
        TotalCoins = CurrentCoins;
    }

    #region Coins and Points
    protected void CalculateTotalCoins()
    {
        TotalCoins = 0;

        foreach (var p in PlacedPieces)
        {
            TotalCoins += p.Definition.cost;
        }
        TotalCoins += CurrentCoins;
    }

    protected void EarnPointsForCapturing(PieceDefinitionSO def)
    {
        CurrentCoins += def.cost;
        CalculateTotalCoins();
    }
    #endregion

    #region Check If Can Do
    public abstract bool CheckCanDoAnything();
    protected abstract bool CheckCanCapture(List<Piece> pieces);
    protected abstract bool CheckCanPromote(List<Piece> pieces);

    protected abstract bool CheckCanPlaceAnyPiece();
    protected abstract bool CheckCanMove(List<Piece> pieces);
    #endregion

    #region Actions
    public void RemovePiece(Piece piece)
    {
        PlacedPieces.Remove(piece);
        CalculateTotalCoins();
    }

    public void OnPromote(Piece old, Piece newPiece)
    {
        PlacedPieces.Add(newPiece);
        PlacedPieces.Remove(old);

        CalculateTotalCoins();
    }
    #endregion
}