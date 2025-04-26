using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public enum PieceColor
    {
        White,
        Gray,
        Black
    }

    public PieceColor color;
    public string position;

    public virtual List<string> GetPossibleMoves(Board board)
    {
        // This is a base implementation, to be overridden by specific pieces
        return new List<string>();
    }
}