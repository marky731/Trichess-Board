# Trichess Board - Piece Movement System

This document explains how the piece movement system works in the Trichess Board project.

## Overview

The piece movement system allows players to select chess pieces and move them to valid positions on the board. The system consists of several components that work together:

1. **PieceController**: Handles piece selection when a piece is clicked
2. **BoardClickHandler**: Handles board clicks and moves selected pieces
3. **PieceMover**: Manages the actual movement of pieces
4. **Board**: Manages the board state and positions
5. **GameManager**: Tracks the currently selected piece

## How It Works

### Piece Selection

1. When a player clicks on a piece, the `PieceController.OnMouseDown()` method is called
2. The PieceController tells the GameManager that this piece was selected using `GameManager.Instance.SelectPiece(piece)`
3. The GameManager highlights the selected piece and stores it in the `selectedPiece` field

### Piece Movement

1. When a player clicks on the board, the `BoardClickHandler.OnMouseDown()` method is called
2. The BoardClickHandler finds the closest valid board position to the clicked point
3. If a piece is selected (via GameManager), it moves that piece to the clicked position
4. The movement is handled by the PieceMover component, which:
   - Updates the piece's position property
   - Updates the piece's GameObject position
   - Updates the board's internal state
   - Handles capturing pieces

## Component Details

### GameManager

- Singleton that manages the game state
- Tracks the currently selected piece
- Handles piece highlighting
- Provides `GetSelectedPiece()` method for other components

### PieceController

- Attached to each chess piece GameObject
- Handles mouse clicks on pieces
- Tells GameManager when a piece is selected

### BoardClickHandler

- Attached to the board GameObject
- Handles mouse clicks on the board
- Finds the closest valid board position to the clicked point
- Moves the selected piece to the clicked position

### PieceMover

- Manages the actual movement of pieces
- Updates the piece's position property
- Updates the piece's GameObject position
- Updates the board's internal state
- Handles capturing pieces

### Board

- Manages the board state and positions
- Provides methods to get positions and fields
- Tracks which pieces are on which fields

## Troubleshooting

If pieces aren't moving correctly, check the following:

1. Make sure GameManager exists in the scene
2. Make sure PieceMover exists in the scene
3. Make sure pieces have PieceController attached
4. Make sure the board has BoardClickHandler attached
5. Check the console for error messages

## Implementation Notes

- The system uses Unity's `FindFirstObjectByType<>()` to find components in the scene
- The GameInitializer creates necessary components if they don't exist
- The system includes fallback mechanisms to handle missing components
