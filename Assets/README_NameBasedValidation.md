# Name-Based Move Validation for Trichess Board

This document explains how to use the name-based move validation system in the Trichess Board project.

## Overview

The name-based move validation system validates chess moves based on the field names (like A7, B2) rather than using complex vector calculations. This approach is more intuitive, easier to debug, and potentially more reliable for the hexagonal chess board.

## Components

The system consists of several components that work together:

1. **NameBasedMoveValidator**: Validates moves based on field names
2. **NameBasedPieceMover**: Moves pieces using the name-based validator
3. **NameBasedBoardClickHandler**: Handles board clicks and uses the name-based piece mover
4. **NameBasedBoardClickHandlerAttacher**: Attaches the name-based board click handler to the board
5. **NameBasedValidationInitializer**: Initializes all the name-based components

## How It Works

### Move Validation

The `NameBasedMoveValidator` validates moves based on the field names:

1. It checks if the source and target positions are valid
2. It checks if the move is valid for the piece type (Rook, Bishop, Knight, etc.)
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

## How to Use

### Option 1: Use the NameBasedValidationInitializer

The easiest way to use the name-based validation system is to add the `NameBasedValidationInitializer` component to a GameObject in your scene:

1. Create a new empty GameObject in your scene
2. Add the `NameBasedValidationInitializer` component to it
3. The initializer will automatically create and set up all the required components

### Option 2: Set Up the Components Manually

If you prefer more control, you can set up the components manually:

1. Create a GameObject for the `NameBasedMoveValidator` and add the component to it
2. Create a GameObject for the `NameBasedPieceMover` and add the component to it
3. Add the `NameBasedBoardClickHandler` component to your board GameObject
4. Initialize the `Chessboard` and set it in the `NameBasedMoveValidator`

## Validation Rules

The name-based validation system implements the following rules for each piece type:

### Rook
- Moves in straight lines along the hexagonal grid
- Can move in the same column, same row, or along the northeast/southwest diagonal

### Bishop
- Moves diagonally along the hexagonal grid
- Can move along the northwest/southeast diagonal or the northeast/southwest diagonal

### Knight
- Makes "L" shaped jumps adapted to the hexagonal grid
- Can move 1 column and 2 rows, 2 columns and 1 row, 2 columns and 2 rows, or 0 columns and 3 rows

### Queen
- Combines the movement of Rook and Bishop
- Can move in straight lines or diagonally

### King
- Moves one space in any direction
- Can move one step horizontally, vertically, or diagonally

### Pawn
- Moves forward one space in the direction specific to its color
- Captures diagonally forward
- Each color (White, Gray, Black) has its own forward direction
- Can move two spaces forward on its first move

## Debugging

The name-based validation system includes extensive debug logging to help you understand what's happening:

- It logs when components are created and initialized
- It logs when moves are validated and why they pass or fail
- It logs when pieces are moved and when turns change

You can use the Unity Console to see these logs and debug any issues.

## Technical Notes

- The `NameBasedMoveValidator` uses the field names (like A7, B2) to validate moves
- The `NameBasedPieceMover` uses the `Board.MovePiece` method to update the board state
- The `NameBasedBoardClickHandler` uses the `FindClosestBoardPosition` method to find the closest board position to a clicked position
- The `NameBasedValidationInitializer` creates and initializes all the components in the correct order
