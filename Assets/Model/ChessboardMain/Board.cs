using Assets.Controller;
// We're explicitly using the Controller version of MoveValidator
using Assets.Model;
using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the chess board, its pieces, and game logic.
/// Handles move validation, check/checkmate detection, and board state.
/// </summary>
public class Board : MonoBehaviour
{
    #region Private Fields
    
    /// <summary>
    /// Stores the positions of all cells on the board (e.g., "A1", "B2").
    /// </summary>
    private Dictionary<string, (double x, double y)> positions;

    /// <summary>
    /// Stores Field objects representing each cell on the board.
    /// </summary>
    private Dictionary<string, Field> fields = new Dictionary<string, Field>();

    /// <summary>
    /// Stores all pieces currently on the board.
    /// </summary>
    private List<Piece> pieces = new List<Piece>();

    /// <summary>
    /// Move validator for checking valid moves.
    /// </summary>
    private readonly Assets.Controller.MoveValidator _validator;

    /// <summary>
    /// Current player's color.
    /// </summary>
    public PieceColor CurrentPlayerColor { get; set; }
    
    #endregion

    #region Constructors
    
    /// <summary>
    /// Default constructor - initializes the board positions.
    /// </summary>
    public Board()
    {
        InitializePositions();
    }

    /// <summary>
    /// Constructor with chessboard parameter - initializes the board and move validator.
    /// </summary>
    /// <param name="chessboard">The chessboard to use for move validation.</param>
    public Board(Assets.Model.ChessboardMain.Pieces.Chessboard chessboard)
    {
        InitializePositions();
        _validator = new Assets.Controller.MoveValidator(chessboard);
    }
    
    #endregion

    #region Initialization
    
    /// <summary>
    /// Initializes the board positions.
    /// </summary>
    private void InitializePositions()
    {
        positions = new Dictionary<string, (double, double)>
        {
            // First block
            { "L8", (2.25, 3.5) }, { "K8", (2.00, 3.10) }, { "J8", (1.85, 2.75) }, { "I8", (1.70, 2.25) },
            { "L7", (2.70, 3.45) }, { "K7", (2.57, 2.97) }, { "J7", (2.45, 2.45) }, { "I7", (2.30, 1.95) },
            { "L6", (3.15, 3.40) }, { "K6", (3.07, 2.84) }, { "J6", (3.05, 2.25) }, { "I6", (2.90, 1.65) },
            { "L5", (3.60, 3.35) }, { "K5", (3.57, 2.72) }, { "J5", (3.55, 2.05) }, { "I5", (3.50, 1.35) },

            // Second block
            { "L9", (4.05, 3.35) }, { "K9", (4.07, 3.10) }, { "J9", (4.05, 2.05) }, { "I9", (4.10, 1.35) },
            { "L10", (4.5, 3.40) }, { "K10", (4.57, 2.97) }, { "J10", (4.55, 2.25) }, { "I10", (4.70, 1.65) },
            { "L11", (4.95, 3.45) }, { "K11", (5.07, 2.84) }, { "J11", (5.05, 2.45) }, { "I11", (5.30, 1.95) },
            { "L12", (5.30, 3.50) }, { "K12", (5.57, 2.72) }, { "J12", (5.55, 2.75) }, { "I12", (5.90, 2.25) },

            // Third block
            { "D8", (1.45, 1.85) }, { "C8", (1.25, 1.50) }, { "B8", (0.95, 1.15) }, { "A8", (0.67, 0.74) },
            { "D7", (2.05, 1.50) }, { "C7", (1.70, 1.15) }, { "B7", (1.30, 0.80) }, { "A7", (0.93, 0.39) },
            { "D6", (2.65, 1.15) }, { "C6", (2.15, 0.80) }, { "B6", (1.65, 0.43) }, { "A6", (1.20, 0.07) },
            { "D5", (3.25, 0.80) }, { "C5", (2.60, 0.45) }, { "B5", (2.00, 0.00) }, { "A5", (1.46, -0.38) },

            // Fourth block
            { "E9", (4.46, 0.80) }, { "F9", (5.05, 0.40) }, { "G9", (5.61, 0.00) }, { "H9", (6.19, -0.38) },
            { "E10", (5.00, 1.15) }, { "F10", (5.50, 0.73) }, { "G10", (5.97, 0.39) }, { "H10", (6.48, -0.02) },
            { "E11", (5.61, 1.50) }, { "F11", (6.00, 1.15) }, { "G11", (6.37, 0.73) }, { "H11", (6.73, 0.37) },
            { "E12", (6.20, 1.85) }, { "F12", (6.50, 1.50) }, { "G12", (6.74, 1.13) }, { "H12", (7.00, 0.74) },

            // Fifth block
            { "H1", (5.45, -2.02) }, { "G1", (4.97, -1.98) }, { "F1", (4.53, -1.95) }, { "E1", (4.05, -1.88) },
            { "H2", (5.62, -1.62) }, { "G2", (5.09, -1.48) }, { "F2", (4.59, -1.36) }, { "E2", (4.06, -1.20) },
            { "H3", (5.78, -1.21) }, { "G3", (5.23, -0.98) }, { "F3", (4.68, -0.76) }, { "E3", (4.10, -0.50) },
            { "H4", (5.99, -0.75) }, { "G4", (5.36, -0.45) }, { "F4", (4.77, -0.13) }, { "E4", (4.13, 0.20) },

            // Sixth block
            { "A4", (1.70, -0.77) }, { "B4", (2.32, -0.42) }, { "C4", (2.93, -0.13) }, { "D4", (3.50, 0.20) },
            { "A3", (1.88, -1.23) }, { "B3", (2.45, -0.92) }, { "C3", (3.00, -0.67) }, { "D3", (3.56, -0.47) },
            { "A2", (2.08, -1.58) }, { "B2", (2.57, -1.42) }, { "C2", (3.07, -1.34) }, { "D2", (3.60, -1.15) },
            { "A1", (2.26, -1.99) }, { "B1", (2.70, -1.92) }, { "C1", (3.17, -1.90) }, { "D1", (3.63, -1.88) }
        };
    }
    
    #endregion

    #region Public Methods
    
    /// <summary>
    /// Gets the position coordinates for a specified cell.
    /// </summary>
    /// <param name="cell">The cell identifier (e.g., "A1", "L12").</param>
    /// <returns>The Vector2 position of the cell, or Vector2.zero if the cell doesn't exist.</returns>
    public Vector2 GetPosition(string cell)
    {
        if (positions.TryGetValue(cell, out var position))
        {
            return new Vector2((float)position.x, (float)position.y);
        }

        Debug.LogError("Invalid position: " + cell);
        return Vector2.zero;
    }

    /// <summary>
    /// Gets the Field object for a specified position.
    /// </summary>
    /// <param name="position">The position identifier (e.g., "A1", "L12").</param>
    /// <returns>The Field object at the specified position, or null if not found.</returns>
    public Field GetField(string position)
    {
        if (fields.TryGetValue(position, out Field field))
        {
            return field;
        }
        Debug.LogError($"Field not found for position: {position}");
        return null;
    }

    /// <summary>
    /// Prints the board cell positions to the debug console.
    /// </summary>
    public void PrintBoard()
    {
        foreach (var kvp in positions)
        {
            Debug.Log($"{kvp.Key}: ({kvp.Value.x}, {kvp.Value.y})");
        }
    }

    /// <summary>
    /// Checks if a field is occupied by a piece.
    /// </summary>
    /// <param name="field">The field to check.</param>
    /// <returns>True if the field is occupied, false otherwise.</returns>
    public bool IsOccupied(Field field)
    {
        return field.OccupiedPiece != null;
    }

    /// <summary>
    /// Checks if a move is valid for a piece.
    /// </summary>
    /// <param name="piece">The piece to move.</param>
    /// <param name="move">The move to validate.</param>
    /// <returns>True if the move is valid, false otherwise.</returns>
    public bool IsValidMove(Piece piece, Move move)
    {
        return _validator.IsValidMove(piece, move);
    }

    /// <summary>
    /// Checks if a player is in checkmate.
    /// </summary>
    /// <param name="playerId">The player ID to check.</param>
    /// <param name="board">The board to check on.</param>
    /// <returns>True if the player is in checkmate, false otherwise.</returns>
    public bool IsCheckmate(int playerId, Board board)
    {
        Piece king = FindKing(playerId);
        if (king == null)
        {
            Debug.Log("Player " + playerId + " has lost their king and is eliminated!");
            return true;
        }

        if (!IsKingInCheck(king, board))
        {
            return false;
        }

        if (HasValidMove(playerId, board))
        {
            return false;
        }

        Debug.Log("Player " + playerId + " is in checkmate!");
        return true;
    }

    /// <summary>
    /// Checks if a move would leave the king in check.
    /// </summary>
    /// <param name="move">The move to check.</param>
    /// <returns>True if the king would be in check after the move, false otherwise.</returns>
    public bool WouldKingBeInCheckAfterMove(Move move)
    {
        // Implementation would go here
        // This is a placeholder for the method referenced in MoveValidator
        return false;
    }

    /// <summary>
    /// Gets the path between two fields.
    /// </summary>
    /// <param name="source">The source field.</param>
    /// <param name="destination">The destination field.</param>
    /// <returns>A list of fields representing the path.</returns>
    public List<Field> GetPath(Field source, Field destination)
    {
        // Implementation would go here
        // This is a placeholder for the method referenced in MoveValidator
        return new List<Field>();
    }

    /// <summary>
    /// Checks if a field is within the bounds of the board.
    /// </summary>
    /// <param name="field">The field to check.</param>
    /// <returns>True if the field is within bounds, false otherwise.</returns>
    public bool IsWithinBounds(Field field)
    {
        // Implementation would go here
        // This is a placeholder for the method referenced in MoveValidator
        return true;
    }
    
    #endregion

    #region Private Methods
    
    /// <summary>
    /// Finds the king piece for a specified player.
    /// </summary>
    /// <param name="playerId">The player ID to find the king for.</param>
    /// <returns>The king piece, or null if not found.</returns>
    private Piece FindKing(int playerId)
    {
        foreach (Piece piece in pieces)
        {
            if (piece is King && piece.PlayerId == playerId)
            {
                return piece;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks if a king is in check.
    /// </summary>
    /// <param name="king">The king piece to check.</param>
    /// <param name="board">The board to check on.</param>
    /// <returns>True if the king is in check, false otherwise.</returns>
    private bool IsKingInCheck(Piece king, Board board)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.PlayerId != king.PlayerId) // Check enemy pieces
            {
                List<string> possibleMoves = piece.GetPossibleMoves(board);
                if (possibleMoves.Contains(king.CurrentPosition))
                {
                    return true; // King is in check
                }
            }
        }
        return false; // King is safe
    }

    /// <summary>
    /// Checks if a player has any valid moves.
    /// </summary>
    /// <param name="playerId">The player ID to check.</param>
    /// <param name="board">The board to check on.</param>
    /// <returns>True if the player has at least one valid move, false otherwise.</returns>
    private bool HasValidMove(int playerId, Board board)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.PlayerId == playerId)
            {
                List<string> possibleMoves = piece.GetPossibleMoves(board);
                if (possibleMoves.Count > 0)
                {
                    return true; // At least one move exists
                }
            }
        }
        return false; // No moves available
    }
    
    #endregion
}
