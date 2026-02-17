using UnityEngine;

public class Move
{
    public Vector2Int From;
    public Vector2Int To;

    public bool IsCapture;
    public Piece CapturedPiece;

    public bool IsPromotion;
}
