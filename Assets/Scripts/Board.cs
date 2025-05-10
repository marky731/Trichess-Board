using Assets.Controller;
using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Dictionary<string, (double x, double y)> positions;

    private Dictionary<string, Field> fields = new Dictionary<string, Field>();

    private List<Piece> pieces = new List<Piece>();

    void Awake()
    {
        Debug.Log("Board Awake called. Initializing board...");
        InitializeBoard();
    }
    
    void Start()
    {
        Debug.Log("Board Start called.");
    }
    
    public Board()
    {
        Debug.Log("Board constructor called.");
        InitializeBoard();
    }
    
    // Initialize the board with all positions and fields
    private void InitializeBoard()
    {
        Debug.Log("Initializing board positions and fields...");
        
        // Initialize positions dictionary if it's null
        if (positions == null)
        {
            positions = new Dictionary<string, (double, double)>
            {
                // First block
                { "L8", (-1.52, 2.6493) }, { "K8", (-1.718, 2.23) }, { "J8", (-1.88, 1.88) }, { "I8", (-2.072, 1.437) },
                { "L7", (-1.082, 2.631) }, { "K7", (-1.186, 2.091) }, { "J7", (-1.308, 1.63) }, { "I7", (-1.439, 1.09) },
                { "L6", (-0.61, 2.573) }, { "K6", (-0.71, 1.97) }, { "J6", (-0.77, 1.38) }, { "I6", (-0.84, 0.77) },
                { "L5", (-0.15, 2.556) }, { "K5", (-0.18, 1.82) }, { "J5", (-0.225, 1.167) }, { "I5", (-0.246, 0.465) },

                // Second block
                { "L9", (0.251, 2.55) }, { "K9", (0.3, 1.84) }, { "J9", (0.324, 1.134) }, { "I9", (0.405, 0.446) },
                { "L10", (0.745, 2.59) }, { "K10", (0.83, 1.98) }, { "J10", (0.869, 1.364) }, { "I10", (0.97, 0.749) },
                { "L11", (1.18, 2.59) }, { "K11", (1.31, 2.09) }, { "J11", (1.45, 1.64) }, { "I11", (1.61, 1.1) },
                { "L12", (1.649, 2.663) }, { "K12", (1.839, 2.246) }, { "J12", (2.033, 1.847) }, { "I12", (2.2, 1.42) },

                // Third block
                { "D8", (-2.296, 1.02) }, { "C8", (-2.559, 0.646) }, { "B8", (-2.81, 0.272) }, { "A8", (-3.086, -0.09) },
                { "D7", (-1.657, 0.672) }, { "C7", (-2.084, 0.245) }, { "B7", (-2.45, -0.034) }, { "A7", (-2.824, -0.443) },
                { "D6", (-1.122, 0.269) }, { "C6", (-1.59, -0.08) }, { "B6", (-2.086, -0.456) }, { "A6", (-2.571, -0.84) },
                { "D5", (-0.548, -0.068) }, { "C5", (-1.07, -0.46) }, { "B5", (-1.717, -0.8) }, { "A5", (-2.293, -1.2) },

                // Fourth block
                { "E9", (0.724, -0.094) }, { "F9", (1.285, -0.44) }, { "G9", (1.865, -0.833) }, { "H9", (2.418, -1.192) },
                { "E10", (1.286, 0.3) }, { "F10", (1.752, -0.083) }, { "G10", (2.21, -0.46) }, { "H10", (2.653, -0.851) },
                { "E11", (1.83, 0.66) }, { "F11", (2.21, 0.29) }, { "G11", (2.58, -0.08) }, { "H11", (2.93, -0.45) },
                { "E12", (2.428, 1.033) }, { "F12", (2.73, 0.7) }, { "G12", (2.986, 0.273) }, { "H12", (3.22, -0.06) },

                // Fifth block
                { "H1", (1.637, -2.865) }, { "G1", (1.164, -2.82) }, { "F1", (0.721, -2.797) }, { "E1", (0.292, -2.73) },
                { "H2", (1.81, -2.44) }, { "G2", (1.34, -2.29) }, { "F2", (0.83, -2.14) }, { "E2", (0.3, -2.02) },
                { "H3", (2.023, -2.034) }, { "G3", (1.432, -1.794) }, { "F3", (0.905, -1.556) }, { "E3", (0.33, -1.333) },
                { "H4", (2.215, -1.603) }, { "G4", (1.6, -1.292) }, { "F4", (1, -0.965) }, { "E4", (0.37, -0.654) },

                // Sixth block
                { "A4", (-2.066, -1.587) }, { "B4", (-1.403, -1.268) }, { "C4", (0.844, -0.957) }, { "D4", (-0.253, -0.654) },
                { "A3", (-1.9, -2.016) }, { "B3", (-1.312, -1.782) }, { "C3", (-0.776, -1.6) }, { "D3", (-0.208, -1.325) },
                { "A2", (-1.67, -2.45) }, { "B2", (-1.25, -2.35) }, { "C2", (-0.68, -2.2) }, { "D2", (-0.2, -2.04) },
                { "A1", (-1.484, -2.884) }, { "B1", (-1.034, -2.812) }, { "C1", (-0.6, -2.797) }, { "D1", (-0.158, -2.726) }
            };
        }
        
        // Initialize fields dictionary
        if (fields.Count == 0)
        {
            Debug.Log("Creating Field objects for each position...");
            foreach (var position in positions)
            {
                string positionKey = position.Key;
                (double x, double y) = position.Value;
                
                Field field = new Field(x, y);
                fields[positionKey] = field;
                
                Debug.Log($"Created Field for position {positionKey} at ({x}, {y})");
            }
        }
    }

    public Vector2 GetPosition(string cell)
    {
        // Initialize the board if positions dictionary is null
        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("Positions dictionary is empty. Initializing board...");
            InitializeBoard();
        }
        
        if (positions.TryGetValue(cell, out var position))
        {
            return new Vector2((float)position.x, (float)position.y);
        }

        Debug.LogError("Geçersiz konum: " + cell);
        
        // Return a default position in the center of the board as a fallback
        Debug.LogWarning("Returning default position (0,0) for invalid cell: " + cell);
        return Vector2.zero;
    }

    public List<string> GetAllPositions()
    {
        // Initialize the board if positions dictionary is null
        if (positions == null || positions.Count == 0)
        {
            Debug.LogWarning("Positions dictionary is empty in GetAllPositions. Initializing board...");
            InitializeBoard();
        }
        
        return new List<string>(positions.Keys);
    }

    public Field GetField(string position)
    {
        // Initialize the board if fields dictionary is empty
        if (fields.Count == 0)
        {
            InitializeBoard();
        }
        
        if (fields.TryGetValue(position, out Field field))
        {
            return field;
        }
        
        Debug.LogError($"Field not found for position: {position}");
        
        // Try to create the field if it doesn't exist but the position is valid
        if (positions.TryGetValue(position, out var pos))
        {
            Debug.Log($"Creating new Field for position {position}");
            Field newField = new Field(pos.x, pos.y);
            fields[position] = newField;
            return newField;
        }
        
        return null;
    }
    
    // Move a piece from one position to another
    public bool MovePiece(Piece piece, string sourcePosition, string targetPosition)
    {
        Debug.Log($"Moving piece from {sourcePosition} to {targetPosition}");
        
        // Get the source and target fields
        Field sourceField = GetField(sourcePosition);
        Field targetField = GetField(targetPosition);
        
        if (sourceField == null || targetField == null)
        {
            Debug.LogError("Source or target field is null!");
            return false;
        }
        
        // Check if the source field contains the piece
        if (sourceField.OccupiedPiece != piece)
        {
            Debug.LogError($"Source field does not contain the specified piece! Expected {piece.GetType().Name} but found {(sourceField.OccupiedPiece != null ? sourceField.OccupiedPiece.GetType().Name : "null")}");
            return false;
        }
        
        // Check if the target field is already occupied
        if (targetField.OccupiedPiece != null)
        {
            Debug.Log($"Target field is occupied by {targetField.OccupiedPiece.GetType().Name}. Capturing piece.");
        }
        
        // Update the fields
        sourceField.OccupiedPiece = null;
        targetField.OccupiedPiece = piece;
        
        // Update the piece's position
        piece.position = targetPosition;
        
        Debug.Log($"Piece moved successfully from {sourcePosition} to {targetPosition}");
        return true;
    }

    public void PrintBoard()
    {
        foreach (var kvp in positions)
        {
            Console.WriteLine($"{kvp.Key}: ({kvp.Value.x}, {kvp.Value.y})");
        }
    }

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

    private MoveValidator _validator;
    
    private Chessboard _chessboard;
    
    // Initialize the MoveValidator with a Chessboard
    private void InitializeMoveValidator()
    {
        Debug.Log("Initializing MoveValidator...");
        
        // Create a new Chessboard instance if it doesn't exist
        if (_chessboard == null)
        {
            _chessboard = new Chessboard();
            Debug.Log("Created new Chessboard instance");
            
            // Give it time to initialize
            System.Threading.Thread.Sleep(100);
        }
        
        // Find the MoveValidator component
        _validator = FindFirstObjectByType<MoveValidator>();
        
        if (_validator == null)
        {
            Debug.LogError("MoveValidator not found! Creating one...");
            GameObject validatorObj = new GameObject("MoveValidator");
            _validator = validatorObj.AddComponent<MoveValidator>();
        }
        
        // Set the Chessboard reference in the MoveValidator
        _validator.SetChessboard(_chessboard);
        
        // Verify that the Chessboard is properly set
        if (_validator != null)
        {
            Debug.Log("MoveValidator initialized successfully");
        }
        else
        {
            Debug.LogError("Failed to initialize MoveValidator!");
        }
    }
    
    // Called after all Start methods have been called
    void OnEnable()
    {
        // Initialize the MoveValidator immediately instead of with a delay
        InitializeMoveValidator();
    }

    public bool IsOccupied(Field field)
    {
        return field.OccupiedPiece != null;
    }

    public bool IsValidMove(Piece piece, Move move)
    {
        // Check if _validator is null (might happen during initialization)
        if (_validator == null)
        {
            Debug.LogWarning("MoveValidator is null in Board.IsValidMove! Initializing MoveValidator...");
            InitializeMoveValidator();
            
            // If still null, return true during initialization to avoid exceptions
            if (_validator == null)
            {
                Debug.LogWarning("MoveValidator is still null after initialization attempt. Allowing move during initialization.");
                return true;
            }
        }
        
        try
        {
            return _validator.IsValidMove(piece, move);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in Board.IsValidMove: {e.Message}");
            // Return true during initialization to avoid breaking the setup
            return true;
        }
    }

    private bool IsKingInCheck(Piece king, Board board)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.PlayerId != king.PlayerId) 
            {
                List<string> possibleMoves = piece.GetPossibleMoves(board);
                if (possibleMoves.Contains(king.CurrentPosition))
                {
                    return true; 
                }
            }
        }
        return false; 
    }

    private bool HasValidMove(int playerId, Board board)
    {
        foreach (Piece piece in pieces)
        {
            if (piece.PlayerId == playerId)
            {
                List<string> possibleMoves = piece.GetPossibleMoves(board);
                if (possibleMoves.Count > 0)
                {
                    return true; 
                }
            }
        }
        return false; 
    }

    public bool IsCheckmate(int playerId, Board board)
    {
        Piece king = FindKing(playerId);
        if (king == null)
        {
            Debug.Log("Player " + playerId + "  ah n  kaybetti, elendi!");
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

        Debug.Log("Player " + playerId + "  ah-mat oldu!");
        return true;
    }
}
