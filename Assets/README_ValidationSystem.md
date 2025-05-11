# Validation System for Trichess Board

This document explains the validation system for the Trichess Board project.

## Overview

The validation system is responsible for validating chess moves according to the rules of three-player hexagonal chess. It ensures that pieces move according to their movement rules, that the path is clear, and that the king is not in check after the move.

## Components

The validation system consists of several components:

1. **FieldDirectionMap**: Stores valid moves for each piece type and position on the hexagonal chess board
2. **NameBasedMoveValidator**: Uses the FieldDirectionMap to validate moves
3. **NameBasedPieceMover**: Moves pieces using the name-based validator
4. **NameBasedBoardClickHandler**: Handles board clicks and uses the name-based piece mover
5. **NameBasedValidationInitializer**: Initializes all the name-based components
6. **ValidationSystemInitializer**: Ensures that the NameBasedValidationInitializer is added to the scene

## How It Works

### Field Direction Map

The `FieldDirectionMap` class is the core of the validation system. It:

1. Generates valid moves for each piece type and position on the board
2. Stores these moves in dictionaries for quick lookup
3. Provides methods to check if a move is valid for a specific piece type

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

### Initialization

The `NameBasedValidationInitializer` initializes all the name-based components:

1. It creates the `NameBasedMoveValidator`
2. It creates the `NameBasedPieceMover`
3. It attaches the `NameBasedBoardClickHandler` to the board
4. It initializes the `FieldDirectionMap` with the board reference

The `ValidationSystemInitializer` ensures that the `NameBasedValidationInitializer` is added to the scene:

1. It checks if the `NameBasedValidationInitializer` already exists
2. If not, it creates a new GameObject and adds the `NameBasedValidationInitializer` component to it

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

## Usage

The validation system is automatically initialized when the game starts. The `GameManager` adds the `ValidationSystemInitializer` component to itself, which in turn adds the `NameBasedValidationInitializer` component to a new GameObject.

## Debugging

The system includes extensive debug logging:

1. It logs when components are created and initialized
2. It logs when moves are validated and why they pass or fail
3. It logs when pieces are moved and when turns change

You can use the Unity Console to see these logs and debug any issues.
