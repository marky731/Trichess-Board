# Three-Player Turn System for Trichess Board

This document explains how the three-player turn system works in the Trichess Board project.

## Overview

The turn system allows three players to take turns moving their chess pieces. The system consists of several components that work together:

1. **TurnManager**: Manages the current player and turn transitions
2. **GameManager**: Tracks the current player and provides access to it
3. **PieceController**: Only allows the current player to select their pieces
4. **BoardClickHandler**: Moves pieces and advances to the next player's turn
5. **TurnIndicator**: Displays whose turn it is in the UI

## How It Works

### Turn Management

1. The game starts with Player 1's turn (white pieces)
2. When a player moves a piece, the turn advances to the next player
3. The turn order is: Player 1 → Player 2 → Player 3 → Player 1 → ...
4. Each player can only select and move their own pieces during their turn

### Player-Piece Assignments

- Player 1 controls the white pieces
- Player 2 controls the gray pieces
- Player 3 controls the black pieces

## Component Details

### TurnManager

- Singleton that manages the current player
- Provides the `NextTurn()` method to advance to the next player
- Triggers the `OnTurnChanged` event when the turn changes
- Provides the `currentPlayer` property to get the current player ID

### GameManager

- Gets the current player from the TurnManager
- Provides the `CurrentPlayerId` property for other components
- Sets the player IDs for all pieces during initialization

### PieceController

- Attached to each chess piece GameObject
- Checks if it's the current player's turn before allowing piece selection
- Only allows players to select their own pieces during their turn

### BoardClickHandler

- Handles mouse clicks on the board
- Moves the selected piece to the clicked position
- Calls `TurnManager.NextTurn()` after a piece is moved

### TurnIndicator

- Displays whose turn it is in the UI
- Updates the display when the turn changes
- Changes color based on the current player (white, gray, or black)

## Setting Up the Turn Indicator UI

1. In the Unity Editor, go to the menu: Trichess > Create Turn Indicator
2. This will create a Canvas with a TurnIndicator UI element
3. The TurnIndicator will automatically update to show whose turn it is

## Implementation Notes

- The system uses Unity's event system to notify components when the turn changes
- The TurnManager is created automatically if it doesn't exist
- Player IDs are assigned to pieces during initialization
- The UI updates automatically when the turn changes
