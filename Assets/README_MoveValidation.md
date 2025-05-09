# Move Validation in Three-Player Chess

This document explains how move validation is implemented in the Trichess Board project.

## Overview

The move validation system ensures that pieces can only make valid moves according to the rules of three-player chess. The system consists of several components that work together:

1. **MoveValidator**: Validates moves according to chess rules
2. **Board**: Manages the board state and delegates validation to MoveValidator
3. **Chessboard**: Provides information about the board layout and piece positions
4. **PieceMover**: Uses the validation system to check if moves are valid before executing them

## How It Works

### Validation Process

1. When a player attempts to move a piece, the `PieceMover` creates a `Move` object
2. The `PieceMover` calls `moveValidator.IsValidMove(piece, move)` to check if the move is valid
3. The `MoveValidator` performs several checks:
   - Is the move allowed by the piece's movement rules?
   - Is the path clear (for pieces like Rooks, Bishops, and Queens)?
   - Is the move within the board boundaries?
   - Is the player moving their own piece?
   - Would the king be in check after the move?
4. If all checks pass, the move is considered valid

### Component Interactions

- **Board**: Initializes a `Chessboard` instance and sets it in the `MoveValidator`
- **MoveValidator**: Uses the `Chessboard` to validate moves
- **PieceMover**: Uses the `MoveValidator` to check if moves are valid before executing them
- **Piece classes**: Implement `GetPossibleMoves` to generate valid moves for each piece type

## Recent Fixes

We've made several important fixes to the move validation system:

1. **Enforced Validation Results**
   - The `PieceMover` now properly prevents invalid moves by returning early when validation fails
   - Previously, invalid moves were logged but still allowed to proceed

2. **Fixed Testing Fallbacks**
   - Changed the fallback behavior in `MoveValidator` to return `false` instead of `true` when there are errors
   - This ensures that moves are prevented when validation cannot be performed properly

3. **Properly Initialized the Chessboard**
   - Added a constructor to `Chessboard` that populates the `fieldMap` with all valid board positions
   - This ensures that the `IsWithinBounds` check works properly

4. **Improved Error Handling**
   - Added more detailed logging for validation failures
   - Added proper error handling in the validation process
   - Prevented moves when exceptions occur during validation

## Implementation Details

### MoveValidator

The `MoveValidator` class is a MonoBehaviour that validates moves according to chess rules:

```csharp
public bool IsValidMove(Piece piece, Move move)
{
    // Check if Chessboard is initialized
    if (_chessboard == null)
    {
        Debug.LogError("Chessboard is null in MoveValidator.IsValidMove! Move validation will fail.");
        return false; // Return false to prevent the move when Chessboard is null
    }

    try
    {
        // Check if the move is allowed by the piece's movement rules
        if (!IsMoveAllowedByPiece(piece, move.Source, move.Destination))
            return false;

        // Check if the path is clear
        if (!IsPathClear(move.Source, move.Destination))
            return false;

        // Check if the move is within the board boundaries
        if (!IsMoveWithinBounds(move))
            return false;

        // Check if the player is moving their own piece
        if (!IsPlayerMovingOwnPiece(move))
            return false;

        // Check if the king would be in check after the move
        if (IsKingInCheckAfterMove(move))
            return false;

        // If all checks pass, the move is valid
        return true;
    }
    catch (Exception e)
    {
        Debug.LogError($"Error in IsValidMove: {e.Message}\n{e.StackTrace}");
        return false; // Return false to prevent the move when there's an error
    }
}
```

### Board

The `Board` class initializes the `MoveValidator` with a `Chessboard` instance:

```csharp
private void InitializeMoveValidator()
{
    Debug.Log("Initializing MoveValidator...");
    
    // Create a new Chessboard instance if it doesn't exist
    if (_chessboard == null)
    {
        _chessboard = new Chessboard();
        Debug.Log("Created new Chessboard instance");
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
    
    Debug.Log("MoveValidator initialized successfully");
}
```

### Chessboard

The `Chessboard` class now initializes the `fieldMap` with all valid positions:

```csharp
// Constructor to initialize the chessboard with all valid positions
public Chessboard()
{
    Debug.Log("Initializing Chessboard with all valid positions...");
    InitializeFields();
}

// Initialize the fieldMap with all valid positions from the Board class
private void InitializeFields()
{
    try
    {
        // Find the Board component
        Board board = UnityEngine.Object.FindFirstObjectByType<Board>();
        if (board == null)
        {
            Debug.LogError("Board not found! Cannot initialize Chessboard.");
            return;
        }

        // Get all positions from the Board
        List<string> allPositions = board.GetAllPositions();
        if (allPositions == null || allPositions.Count == 0)
        {
            Debug.LogError("No positions found in Board! Cannot initialize Chessboard.");
            return;
        }

        // Create a Field for each position and add it to the fieldMap
        foreach (string position in allPositions)
        {
            Vector2 pos = board.GetPosition(position);
            Field field = new Field(pos.x, pos.y);
            fieldMap[field] = null;
        }
    }
    catch (Exception e)
    {
        Debug.LogError($"Error initializing Chessboard: {e.Message}\n{e.StackTrace}");
    }
}
```

### PieceMover

The `PieceMover` class now properly prevents invalid moves:

```csharp
try
{
    Debug.Log($"Validating move from {piece.position} to {targetPosition}...");
    if (!moveValidator.IsValidMove(piece, move))
    {
        Debug.LogError($"Invalid move from {piece.position} to {targetPosition}! The move does not comply with chess rules.");
        
        // Prevent invalid moves
        return; // This line is now uncommented to prevent invalid moves
    }
    else
    {
        Debug.Log($"Move from {piece.position} to {targetPosition} is valid.");
    }
}
catch (Exception e)
{
    Debug.LogError($"Error validating move: {e.Message}\n{e.StackTrace}");
    Debug.LogWarning("Cannot validate move due to an error. Preventing the move for safety.");
    return; // Prevent the move when there's an error in validation
}
```

## Hexagonal Board Considerations

The validation system is adapted for the hexagonal board:

- Each piece type has specific movement rules adapted for the hexagonal grid
- The `Direction` class defines vectors for each direction on the hexagonal grid
- The `Chessboard.IsWithinBounds` method checks if a move is within the hexagonal board boundaries
- The `Chessboard.GetPath` method finds the path between two positions on the hexagonal grid

## Error Handling

The validation system now includes robust error handling:

- Null checks for all components
- Try-catch blocks to handle exceptions
- Detailed logging for debugging
- Moves are prevented when validation fails or when exceptions occur

## Usage

The validation system is used automatically when moving pieces:

1. When a player clicks on a piece and then clicks on a destination, the `BoardClickHandler` calls `pieceMover.MovePiece(piece, targetPosition)`
2. The `PieceMover` validates the move using the `MoveValidator`
3. If the move is valid, the piece is moved to the destination
4. If the move is invalid, an error is logged and the move is prevented

## Technical Notes

- The `MoveValidator` is initialized by the `Board` class after all components have started
- The `Chessboard` instance is created by the `Board` class and set in the `MoveValidator`
- The validation system can be disabled for testing by setting `skipValidation = true` in the `PieceMover` class, but this is not recommended for production
