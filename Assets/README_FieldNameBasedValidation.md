# Field Name-Based Validation for Trichess Board

This document explains the field name-based validation system implemented for the Trichess Board project.

## Overview

The field name-based validation system validates chess moves based on the field names (like A7, B2) rather than using vector calculations. This approach is more intuitive, easier to debug, and more reliable for the hexagonal chess board.

## Components

The system consists of several components:

1. **FieldDirectionMap**: Stores valid moves for each piece type and position on the hexagonal chess board
2. **NameBasedMoveValidator**: Uses the FieldDirectionMap to validate moves
3. **NameBasedPieceMover**: Moves pieces using the name-based validator
4. **NameBasedBoardClickHandler**: Handles board clicks and uses the name-based piece mover
5. **NameBasedValidationInitializer**: Initializes all the name-based components

## How It Works

### Field Direction Map

The `FieldDirectionMap` class is the core of the field name-based validation system. It:

1. Generates valid moves for each piece type and position on the board
2. Stores these moves in dictionaries for quick lookup
3. Provides methods to check if a move is valid for a specific piece type

For example, for a pawn at position A2:
- Regular moves: A3, A4 (if it's the first move)
- Capture moves: B3, Z3 (if these positions exist on the board)

### Move Validation

The `NameBasedMoveValidator` uses the `FieldDirectionMap` to validate moves:

1. It checks if the source and target positions are valid
2. It checks if the move is valid for the piece type using the `FieldDirectionMap`
3. It checks if the path is clear
4. It checks if the player is moving their own piece
5. It checks if the king would be in check after the move

### Piece Movement

The `NameBasedPieceMover` moves pieces using the name-based validator:

1. It gets the source and target positions
2. It validates the move using the `NameBasedMoveValidator`
3. If the move is valid, it moves the piece to the target position
4. It advances to the next player's turn

### Board Click Handling

The `NameBasedBoardClickHandler` handles board clicks:

1. It finds the closest board position to the clicked position
2. If a piece is selected, it moves the piece to the target position using the `NameBasedPieceMover`

## Pawn Movement Rules

The pawn movement rules are defined specifically for each player in the three-player hexagonal chess:

### White Pawns (Player 1)
- Move in the same letter (column) and decrease the number
- Special exceptions:
  - White pawn at D5 can go to E9
  - White pawn at I5 can go to E4
  - White pawn at D4 can go to I9
  - White pawn at E4 can go to I5

### Gray Pawns (Player 2)
- Move in the same letter (column) and increase the number

### Black Pawns (Player 3)
- Move in the same letter (column) and decrease the number
- Special exceptions:
  - Black pawn at I9 can go to D4
  - Black pawn at E9 can go to D5

## Other Piece Movement Rules

### Rook
- Moves in straight lines (horizontally, vertically, and one diagonal in hexagonal chess)
- Can move any number of steps in these directions

### Bishop
- Moves in diagonal lines (two of the three diagonals in hexagonal chess)
- Can move any number of steps in these directions

### Knight
- Makes "L" shaped jumps adapted to the hexagonal grid
- Has 12 possible move directions

### Queen
- Combines the movement of Rook and Bishop
- Can move in all directions

### King
- Moves one step in any direction
- Can move in all directions

## Implementation Details

### Field Direction Map Generation

The `FieldDirectionMap` generates valid moves for each piece type and position:

1. It iterates through all positions on the board
2. For each position, it generates valid moves for each piece type
3. It stores these moves in dictionaries for quick lookup

### Path Checking

The system checks if the path between the source and target positions is clear:

1. It gets all positions between the source and target
2. It checks if any of these positions are occupied
3. If any position is occupied, the path is not clear

### Capture Handling

The system handles captures differently for pawns:

1. Pawns can only capture diagonally, and only when there's an enemy piece to capture
2. Pawns cannot move diagonally to an empty square
3. Other pieces can capture any enemy piece in their path

## Usage

To use the field name-based validation system:

1. Add the `NameBasedValidationInitializer` component to a GameObject in your scene
2. The initializer will automatically create and set up all the required components
3. The system will validate moves based on field names instead of vector calculations

## Debugging

The system includes extensive debug logging:

1. It logs when components are created and initialized
2. It logs when moves are validated and why they pass or fail
3. It logs when pieces are moved and when turns change

You can use the Unity Console to see these logs and debug any issues.

## Technical Notes

- The `FieldDirectionMap` is a singleton to ensure there's only one instance
- The `NameBasedMoveValidator` uses the `FieldDirectionMap` to validate moves
- The `NameBasedPieceMover` uses the `Board.MovePiece` method to update the board state
- The `NameBasedBoardClickHandler` uses the `FindClosestBoardPosition` method to find the closest board position to a clicked position
- The `NameBasedValidationInitializer` creates and initializes all the components in the correct order
