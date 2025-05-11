using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Controller
{
    public class NameBasedPieceMover : MonoBehaviour
    {
        // Name-based move validator
        private NameBasedMoveValidator moveValidator;

        // Turn manager
        private TurnManager turnManager;

        // Board
        private Board board;

        // Initialize components
        void Start()
        {
            moveValidator = FindFirstObjectByType<NameBasedMoveValidator>();
            
            // If the validator doesn't exist, create it
            if (moveValidator == null)
            {
                Debug.LogWarning("NameBasedMoveValidator not found! Creating one...");
                GameObject validatorObj = new GameObject("NameBasedMoveValidator");
                moveValidator = validatorObj.AddComponent<NameBasedMoveValidator>();
            }
            
            // Use the singleton instance if available
            turnManager = TurnManager.Instance;
            if (turnManager == null)
            {
                turnManager = FindFirstObjectByType<TurnManager>();
                Debug.LogWarning("TurnManager.Instance was null, found instance through FindFirstObjectByType");
            }
            
            board = FindFirstObjectByType<Board>();
            
            Debug.Log("NameBasedPieceMover initialized");
        }

        // Move a piece to a target position
        public void MovePiece(Piece piece, string targetPosition)
        {
            Debug.Log($"MovePiece called: Moving {piece.GetType().Name} from {piece.position} to {targetPosition}");
            
            // Check if board is null
            if (board == null)
            {
                Debug.LogError("Board is null in MovePiece!");
                board = FindFirstObjectByType<Board>();
                if (board == null)
                {
                    Debug.LogError("Could not find Board component!");
                    return;
                }
            }
            
            // Check if piece is null
            if (piece == null)
            {
                Debug.LogError("Piece is null in MovePiece!");
                return;
            }
            
            // Check if piece.GameObject is null
            if (piece.GameObject == null)
            {
                Debug.LogError("Piece.GameObject is null! Trying to find it...");
                piece.GameObject = piece.gameObject;
                if (piece.GameObject == null)
                {
                    Debug.LogError("Could not find piece GameObject!");
                    return;
                }
            }
            
            // Get the source position
            string sourcePosition = piece.position;
            
            // If piece.position is null or empty, try to find the current position
            if (string.IsNullOrEmpty(sourcePosition))
            {
                Debug.LogWarning("Piece position is null or empty. Trying to determine current position...");
                
                // Find the current position based on the GameObject's transform
                sourcePosition = FindClosestBoardPosition(board, piece.GameObject.transform.position);
                if (sourcePosition != null)
                {
                    // Update the piece's position property
                    piece.position = sourcePosition;
                    Debug.Log($"Updated piece position to: {sourcePosition}");
                }
                else
                {
                    Debug.LogError("Could not determine the current position of the piece!");
                    return;
                }
            }
            
            // Validate the move using the name-based validator
            bool skipValidation = false; // Validation is enabled
            if (moveValidator != null && !skipValidation)
            {
                try
                {
                    Debug.Log($"Validating move from {sourcePosition} to {targetPosition}...");
                    if (!moveValidator.IsValidMove(piece, sourcePosition, targetPosition))
                    {
                        Debug.LogError($"Invalid move from {sourcePosition} to {targetPosition}! The move does not comply with chess rules.");
                        
                        // Prevent invalid moves
                        return;
                    }
                    else
                    {
                        Debug.Log($"Move from {sourcePosition} to {targetPosition} is valid.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error validating move: {e.Message}\n{e.StackTrace}");
                    Debug.LogWarning("Cannot validate move due to an error. Preventing the move for safety.");
                    return; // Prevent the move when there's an error in validation
                }
            }
            else
            {
                if (moveValidator == null)
                {
                    Debug.LogWarning("NameBasedMoveValidator is null! Trying to find it...");
                    moveValidator = FindFirstObjectByType<NameBasedMoveValidator>();
                    
                    if (moveValidator == null)
                    {
                        Debug.LogError("Could not find NameBasedMoveValidator component! Creating one...");
                        GameObject validatorObj = new GameObject("NameBasedMoveValidator");
                        moveValidator = validatorObj.AddComponent<NameBasedMoveValidator>();
                    }
                }
                
                if (skipValidation)
                {
                    Debug.LogWarning("Validation is skipped. This should be enabled in production.");
                }
            }
            
            // Get the source and target fields
            Field sourceField = board.GetField(sourcePosition);
            Field targetField = board.GetField(targetPosition);
            
            if (sourceField == null || targetField == null)
            {
                Debug.LogError($"Source or target field is null! Source: {sourcePosition}, Target: {targetPosition}");
                return;
            }
            
            // Check if there's a piece at the target position
            Piece takenPiece = targetField.OccupiedPiece;
            if (takenPiece != null)
            {
                Debug.Log($"Taking piece: {takenPiece.GetType().Name}");
            }
            
            // Move the piece
            try
            {
                Vector2 newPosition = board.GetPosition(targetPosition);
                Debug.Log($"Moving piece to position: ({newPosition.x}, {newPosition.y})");
                
                // Use the Board.MovePiece method to update the fields
                bool moveSuccess = board.MovePiece(piece, sourcePosition, targetPosition);
                
                if (!moveSuccess)
                {
                    Debug.LogError("Failed to update board fields for the move!");
                    // Continue anyway for testing
                }
                
                // Update the GameObject's position
                piece.GameObject.transform.position = new Vector3(newPosition.x, newPosition.y, piece.GameObject.transform.position.z);
                
                Debug.Log($"Piece moved to {targetPosition}");
                
                // If there was a piece at the target position, capture it
                if (takenPiece != null && takenPiece.GameObject != null)
                {
                    Debug.Log($"Capturing piece: {takenPiece.GetType().Name}");
                    
                    // Disable the GameObject to make it disappear
                    takenPiece.GameObject.SetActive(false);
                    
                    // Remove the piece from the game state
                    // This is already handled by the Board.MovePiece method which sets targetField.OccupiedPiece = piece
                    
                    // You could add additional logic here for tracking captured pieces if needed
                    // For example, adding to a list of captured pieces for each player
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error moving piece: {e.Message}");
                return;
            }
            
            // Track if the move was successful
            bool moveSuccessful = true;
            
            // Advance to the next player's turn
            if (moveSuccessful && turnManager != null)
            {
                try
                {
                    Debug.Log($"Move successful, advancing turn from Player {turnManager.currentPlayer}");
                    turnManager.NextTurn();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error changing turn: {e.Message}");
                    // Continue anyway, as the piece has already moved
                }
            }
            else if (turnManager == null)
            {
                Debug.LogWarning("TurnManager is null, trying to find it...");
                turnManager = TurnManager.Instance;
                if (turnManager == null)
                {
                    turnManager = FindFirstObjectByType<TurnManager>();
                    if (turnManager != null)
                    {
                        Debug.Log("Found TurnManager, advancing turn");
                        turnManager.NextTurn();
                    }
                    else
                    {
                        Debug.LogError("Could not find TurnManager! Turn will not advance.");
                    }
                }
                else
                {
                    Debug.Log("Found TurnManager.Instance, advancing turn");
                    turnManager.NextTurn();
                }
            }
            
            Debug.Log("MovePiece completed successfully");
        }
        
        // Helper method to find the closest board position to a world position
        private string FindClosestBoardPosition(Board board, Vector3 worldPosition)
        {
            // Get all board positions
            var positions = board.GetAllPositions();
            
            if (positions == null || positions.Count == 0)
            {
                Debug.LogError("No board positions available");
                return null;
            }
            
            string closestPosition = null;
            float closestDistance = float.MaxValue;
            
            // Find the closest position
            foreach (var position in positions)
            {
                Vector2 positionVector = board.GetPosition(position);
                float distance = Vector2.Distance(new Vector2(worldPosition.x, worldPosition.y), positionVector);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = position;
                }
            }
            
            // Only return if the closest position is within a reasonable distance
            if (closestDistance <= 2.0f)
            {
                // Debug.Log($"Found position {closestPosition} at distance {closestDistance}");
                return closestPosition;
            }
            
            Debug.Log($"No position found within threshold. Closest was {closestPosition} at distance {closestDistance}");
            
            return null;
        }
    }
}
