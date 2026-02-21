using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : BasePlayerController<AIController>
{
    private Tile[] tiles;

    #region Unity Default Methods
    public override void OnInitAwake()
    {
        base.OnInitAwake();
    }

    public override void OnInitStart()
    {
        base.OnInitStart();
    }
    #endregion

    #region Check If Can Do
    public override bool CheckCanDoAnything()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (CheckCanCapture(aiPieces)) return true;

        if (CheckCanPromote(aiPieces)) return true;

        if (CheckCanPlaceAnyPiece()) return true;

        if (CheckCanMove(aiPieces)) return true;

        return false;
    }

    protected override bool CheckCanCapture(List<Piece> pieces)
    {
        List<(Piece piece, Tile target, int value)> captures = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
            {
                Piece targetPiece = tile.Piece;
                int value = targetPiece.Definition.cost;

                captures.Add((piece, tile, value));
            }
        }

        return captures.Count > 0;
    }

    protected override bool CheckCanPromote(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.CanPromote)
            {
                return true;
            }
        }

        return false;
    }

    protected override bool CheckCanPlaceAnyPiece()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        if (freeTiles.Count == 0)
            return false;

        PieceDefinitionSO pieceDef = ChoosePiece();
        if (pieceDef == null)
            return false;

        Tile tile = BoardController.Instance.ChooseTileToInstantiateNewPiece(freeTiles, pieceDef, false);
        if (tile == null)
            return false;

        return true;
    }

    protected override bool CheckCanMove(List<Piece> pieces)
    {
        List<(Piece piece, Tile tile)> moves = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
            {
                moves.Add((piece, tile));
            }
        }

        return moves.Count > 0;
    }
    #endregion

    #region Actions
    public void Play()
    {
        Invoke(nameof(PlayTurn), 5f);
    }

    private void PlayTurn()
    {
        List<Piece> aiPieces = GetAIPieces();

        if (TryCapture(aiPieces)) return;

        if (TryPromote(aiPieces)) return;

        if (CurrentCoins > 0 && TryPlacementDuringTurn()) return;

        DoRandomMove(aiPieces);
    }

    private List<Piece> GetAIPieces()
    {
        List<Piece> pieces = new();

        if (tiles == null || tiles.Length <= 0)
        {
            tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        }

        foreach (Tile tile in tiles)
        {
            if (tile.IsOccupied && !tile.Piece.IsFromPlayer)
                pieces.Add(tile.Piece);
        }

        return pieces;
    }

    private bool TryCapture(List<Piece> pieces)
    {
        List<(Piece piece, Tile target, int value)> captures = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidCaptures(BoardController.Instance))
            {
                Piece targetPiece = tile.Piece;
                int value = targetPiece.Definition.cost;

                captures.Add((piece, tile, value));
            }
        }

        if (captures.Count == 0)
            return false;

        int bestValue = captures.Max(c => c.value);

        var bestCaptures = captures
            .Where(c => c.value == bestValue)
            .ToList();

        var chosen = bestCaptures[UnityEngine.Random.Range(0, bestCaptures.Count)];

        ExecuteMove(chosen.piece, chosen.target);

        return true;
    }

    private bool TryPromote(List<Piece> pieces)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(piece);
                return true;
            }
        }

        return false;
    }

    private bool TryPlacementDuringTurn()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        if (freeTiles.Count == 0)
            return false;

        PieceDefinitionSO pieceDef = ChoosePiece();
        if (pieceDef == null)
            return false;

        Tile tile = BoardController.Instance.ChooseTileToInstantiateNewPiece(freeTiles, pieceDef, false);
        if (tile == null)
            return false;

        PlacePiece(tile, pieceDef, true);

        ChooseGameMode.Instance.CurrentGameMode.SetGameTurn(GameTurn.PlayerTurn);
        return true;
    }

    private void DoRandomMove(List<Piece> pieces)
    {
        List<(Piece piece, Tile tile)> moves = new();

        foreach (Piece piece in pieces)
        {
            foreach (Tile tile in piece.GetValidMoves(BoardController.Instance))
            {
                moves.Add((piece, tile));
            }
        }

        if (moves.Count == 0)
            return;

        var chosen = moves[Random.Range(0, moves.Count)];
        ExecuteMove(chosen.piece, chosen.tile);
    }

    private void ExecuteMove(Piece piece, Tile target)
    {
        Tile origin = piece.CurrentTile;

        if (target.IsOccupied)
        {
            EarnPointsForCapturing(target.Piece.Definition);
            target.Piece.OnCaptured();
        }

        origin.Clear();
        target.SetPiece(piece);

        piece.MoveToTile(target, 0.35f, () =>
        {
            if (piece.CanPromote)
            {
                PromotionController.Instance.RequestPromotion(piece);
            }
            else
            {
                ChooseGameMode.Instance.CurrentGameMode.SetGameTurn(GameTurn.PlayerTurn);
            }
        });
    }

    public void StartPlacement()
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        while (CurrentCoins > 0 && freeTiles.Count > 0)
        {
            PieceDefinitionSO pieceDef = ChoosePiece();
            if (pieceDef == null) break;

            Tile tile = BoardController.Instance.ChooseTileToInstantiateNewPiece(freeTiles, pieceDef, false);
            if (tile == null) break;

            Piece piece = PlacePiece(tile, pieceDef, false);
            freeTiles.Remove(tile);
            PlacedPieces.Add(piece);
        }

        ChooseGameMode.Instance.CurrentGameMode.SetGameTurn(GameTurn.PlayerPlacement);
    }

    private PieceDefinitionSO ChoosePiece()
    {
        List<PieceDefinitionSO> possible = PhaseController.Instance.CurrentPhase.availablePiecesAI.FindAll(p => p.cost <= CurrentCoins);

        if (possible.Count == 0)
            return null;

        return possible[Random.Range(0, possible.Count)];
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool show)
    {
        CurrentCoins -= def.cost;

        Piece piece = Instantiate(def.prefab);
        piece.Initialize(def, false);
        tile.SetPiece(piece);

        piece.SetVisible(show);
        tile.SetOccupiedMarker(!show);

        return piece;
    }
    #endregion
}
