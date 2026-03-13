using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingStateMode : GameModeBase
{
    private King playerKing;
    private KingState kingState;

    public List<KingStateDefinitionSO> KingStateDefinitions;

    public override void StartMode()
    {
        base.StartMode();
        SetGameTurn(GameTurn.OpponentPlacement);
    }

    public override void SetGameTurn(GameTurn newPhase)
    {
        switch (newPhase)
        {
            case GameTurn.OpponentPlacement:
                ChooseGameModeUI.Instance.ShowScreen(GameScreen.Play);
                kingState = GeneratePhase(PhaseController.Instance.CurrentPhase);
                break;
            case GameTurn.GameOverWin:
                PhaseController.Instance.ShowNextPhaseButton(true);
                break;
            case GameTurn.GameOverLost:
                PhaseController.Instance.ShowNextPhaseButton(false);
                break;
        }

        ChooseGameModeUI.Instance.CurrentGameMode.SetOptions(KingStateDefinitions);
        ChooseGameModeUI.Instance.CurrentGameMode.SetGameTurnText(newPhase);
    }

    public override void PlayerFinishedMoves()
    {

    }

    protected override bool CheckAIWin()
    {
        return false;
    }

    protected override bool CheckPlayerWin()
    {
        return false;
    }

    public void OnPlayerGuess(KingState guessedType)
    {
        if (guessedType == kingState)
        {
            SetGameTurn(GameTurn.GameOverWin);
        }
        else
        {
            SetGameTurn(GameTurn.GameOverLost);
        }
    }

    public KingState GeneratePhase(PhaseSO phase)
    {
        BoardController.Instance.ClearBoardPlayer(false);

        KingState desired = PickDesiredState(phase);

        PuzzleTemplateSO template = PickTemplate(phase, desired);

        if (template == null)
        {
            Debug.LogError($"No template for state {desired} in phase {phase.phase}");
            return KingState.Safe;
        }

        SpawnTemplate(template);

        return playerKing.EvaluateKingState();
    }

    private KingState PickDesiredState(PhaseSO phase)
    {
        int roll = Random.Range(0, 100);

        if (roll < phase.safeChance)
            return KingState.Safe;

        roll -= phase.safeChance;

        if (roll < phase.checkChance)
            return KingState.Check;

        roll -= phase.checkChance;

        if (roll < phase.checkmateChance)
            return KingState.Checkmate;

        return KingState.Stalemate;
    }

    private PuzzleTemplateSO PickTemplate(PhaseSO phase, KingState state)
    {
        List<PuzzleTemplateSO> list = state switch
        {
            KingState.Safe => phase.templatesSafe,
            KingState.Check => phase.templatesCheck,
            KingState.Checkmate => phase.templatesMate,
            KingState.Stalemate => phase.templatesStale,
            _ => null
        };

        if (list == null || list.Count == 0)
            return null;

        return list[Random.Range(0, list.Count)];
    }

    private void SpawnTemplate(PuzzleTemplateSO template)
    {
        bool hasPawn = template.pieces.Any(p => p.pieceOptions.Any(def => def.type == PieceType.Pawn));
        int transform = hasPawn ? Random.Range(0, 2) : Random.Range(0, 8);

        bool spawnInversedColors = Random.Range(0, 2) == 1;

        foreach (var p in template.pieces)
        {
            if (Random.Range(0, 100) > p.spawnChance)
                continue;

            PieceDefinitionSO piece = p.pieceOptions[Random.Range(0, p.pieceOptions.Count)];

            Tile tile;

            if (p.constraint == PositionConstraint.Fixed)
            {
                int index = Random.Range(0, p.positions.Count);
                Vector2Int pos = Transform(p.positions[index], transform);
                tile = BoardController.Instance.GetTile(pos.x, pos.y);
            }
            else
            {
                tile = FindTileForConstraint(p.constraint);
            }

            if (tile == null || tile.IsOccupied) continue;

            PlacePiece(tile, piece, p.isPlayer, spawnInversedColors);
        }
    }

    private Tile FindTileForConstraint(PositionConstraint constraint)
    {
        List<Tile> freeTiles = BoardController.Instance.GetAllFreeTiles();

        List<Tile> candidates = new();

        foreach (Tile tile in freeTiles)
        {
            Vector2Int pos = tile.Position;

            switch (constraint)
            {
                case PositionConstraint.Any:
                    candidates.Add(tile);
                    break;

                case PositionConstraint.Center:
                    if (pos.x >= 2 && pos.x <= 5 && pos.y >= 2 && pos.y <= 5)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.Edge:
                    if (pos.x == 0 || pos.x == 7 || pos.y == 0 || pos.y == 7)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.Corner:
                    if ((pos.x == 0 || pos.x == 7) && (pos.y == 0 || pos.y == 7))
                        candidates.Add(tile);
                    break;

                case PositionConstraint.NearPlayerKing:
                    if (playerKing != null &&
                        Vector2Int.Distance(pos, playerKing.CurrentTile.Position) <= 2)
                        candidates.Add(tile);
                    break;

                case PositionConstraint.FarFromPlayerKing:
                    if (playerKing != null &&
                        Vector2Int.Distance(pos, playerKing.CurrentTile.Position) >= 4)
                        candidates.Add(tile);
                    break;
            }
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    private Piece PlacePiece(Tile tile, PieceDefinitionSO def, bool isFromPlayer, bool isInversed)
    {
        Piece piece = Instantiate(def.prefab);

        piece.Initialize(def, isFromPlayer ^ isInversed);

        tile.SetPiece(piece);

        if (piece.IsFromPlayer ^ isInversed && piece is King king)
        {
            playerKing = king;
            king.CurrentTile.SetOccupiedMarker(true);
        }

        return piece;
    }

    public static Vector2Int Transform(Vector2Int pos, int type)
    {
        int x = pos.x;
        int y = pos.y;

        return type switch
        {
            0 => new Vector2Int(x, y),
            1 => new Vector2Int(7 - x, y),
            2 => new Vector2Int(x, 7 - y),
            3 => new Vector2Int(7 - x, 7 - y),
            4 => new Vector2Int(y, x),
            5 => new Vector2Int(7 - y, x),
            6 => new Vector2Int(y, 7 - x),
            7 => new Vector2Int(7 - y, 7 - x),
            _ => pos,
        };
    }
}
