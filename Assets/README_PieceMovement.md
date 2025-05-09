# Piece Movement in Three-Player Chess

This document explains how piece movement is implemented in the Trichess Board project.

## Overview

The piece movement system allows players to select and move their chess pieces on the hexagonal board. The system consists of several components that work together:

1. **PieceController**: Handles piece selection when a piece is clicked
2. **BoardClickHandler**: Handles clicks on the board and moves the selected piece
3. **PieceMover**: Handles the actual movement of pieces
4. **GameManager**: Manages the game state and piece selection
5. **TurnManager**: Manages whose turn it is

## How It Works

### Piece Selection

1. When a player clicks on a piece, the `PieceController` checks if it's that player's turn
2. If it is, the piece is selected and highlighted
3. The `GameManager` keeps track of the selected piece

### Piece Movement

1. When a player clicks on the board, the `BoardClickHandler` finds the closest board position
2. If a piece is selected, it moves the piece to that position using the `PieceMover`
3. The `PieceMover` updates the piece's position and handles capturing enemy pieces
4. After a piece is moved, the turn advances to the next player

### Movement Validation

1. The `MoveValidator` checks if a move is valid according to the chess rules
2. It checks if the move is allowed by the piece's movement rules
3. It checks if the path is clear (for pieces like Rooks, Bishops, and Queens)
4. It checks if the move is within the board boundaries

## Piece Movement Rules

Each piece type has specific movement rules adapted for the hexagonal board:

### Pawn
- Moves forward one space in the direction specific to its color
- Captures diagonally forward
- Each color (White, Gray, Black) has its own forward direction

### Rook
- Moves in straight lines along the hexagonal grid
- Can move in six directions (forward, backward, and the four diagonal directions)

### Bishop
- Moves diagonally along the hexagonal grid
- Can move in six diagonal directions

### Knight
- Makes "L" shaped jumps adapted to the hexagonal grid
- Has 12 possible move directions

### Queen
- Combines the movement of Rook and Bishop
- Can move in all 12 directions (6 straight and 6 diagonal)

### King
- Moves one space in any direction
- Can move in all 12 directions (6 straight and 6 diagonal)

## Direction System

The direction system is adapted for the hexagonal board:

- Each color (White, Gray, Black) has its own set of directions
- The `Direction` class defines vectors for each direction
- The `StraightDirections` and `DiagonalDirections` classes define sets of directions for different piece types

## Implementation Details

### Coordinate System

- The board uses a coordinate system with positions like "A1", "B2", etc.
- Each position has corresponding (x, y) coordinates in world space
- The `Board` class maps between position strings and world coordinates

### Path Finding

- The `GetPath` method in the `Chessboard` class finds the path between two positions
- It uses a line-drawing algorithm adapted for the hexagonal grid
- It finds all intermediate positions along the path

### Move Validation

- The `IsValidMove` method in the `MoveValidator` class checks if a move is valid
- It checks if the move is allowed by the piece's movement rules
- It checks if the path is clear
- It checks if the move is within the board boundaries

## Usage

To move a piece:

1. Click on one of your pieces to select it
2. Click on a valid destination on the board
3. The piece will move to that position if the move is valid
4. The turn will advance to the next player

## Technical Notes

- The `PieceMover` class handles the actual movement of pieces
- The `Board` class manages the board state and piece positions
- The `GameManager` class manages the game state and piece selection
- The `TurnManager` class manages whose turn it is
