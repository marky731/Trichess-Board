using UnityEngine;
using Assets.Model.ChessboardMain.Pieces;
using Assets.Controller;

public class BoardClickHandler : MonoBehaviour
{
    private Board board;
    private PieceMover pieceMover;
    private BoxCollider2D boardCollider;

    void Start()
    {
        board = FindFirstObjectByType<Board>();
        pieceMover = FindFirstObjectByType<PieceMover>();
        
        if (board == null)
        {
            Debug.LogError("Board not found!");
        }
        
        if (pieceMover == null)
        {
            Debug.LogError("PieceMover not found!");
        }
        
        // Add a BoxCollider2D to the board if it doesn't have one
        boardCollider = GetComponent<BoxCollider2D>();
        if (boardCollider == null)
        {
            boardCollider = gameObject.AddComponent<BoxCollider2D>();
            // Set the size of the collider to cover the entire board
            boardCollider.size = new Vector2(10, 10); // Adjust size as needed
            Debug.Log("Added BoxCollider2D to the board");
        }
    }

    void OnMouseDown()
    {
        // Check if Camera.main is not null
        if (Camera.main == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }
        
        // Get the clicked position in world space
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // Ensure z is 0 for 2D

        Debug.Log($"Board clicked at position: {clickPosition}");

        // Check if board is not null
        if (board == null)
        {
            Debug.LogError("Board is null! Cannot find closest position.");
            board = FindFirstObjectByType<Board>();
            if (board == null)
            {
                Debug.LogError("Could not find Board component!");
                return;
            }
        }

        // Find the closest board position
        string targetPosition = FindClosestBoardPosition(clickPosition);
        
        if (targetPosition != null)
        {
            Debug.Log($"Closest board position: {targetPosition}");
            
            // Check if GameManager.Instance is not null
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null! Make sure GameManager exists in the scene.");
                return;
            }
            
            // If a piece is selected, move it to the target position
            Piece selectedPiece = GameManager.Instance.GetSelectedPiece();
            if (selectedPiece != null)
            {
                Debug.Log($"Moving piece {selectedPiece.GetType().Name} to {targetPosition}");
                
                // Find the current position of the piece based on its GameObject position
                string currentPosition = FindClosestBoardPosition(selectedPiece.GameObject.transform.position);
                if (currentPosition == null)
                {
                    Debug.LogError("Could not determine the current position of the piece!");
                    return;
                }
                
                // Update the piece's position property
                selectedPiece.position = currentPosition;
                
                Debug.Log($"Determined piece's current position: {currentPosition}");
                Debug.Log($"Moving piece {selectedPiece.GetType().Name} from {currentPosition} to {targetPosition}");
                
                // Check if pieceMover is not null
                if (pieceMover == null)
                {
                    Debug.LogWarning("PieceMover is null! Trying to find it...");
                    pieceMover = FindFirstObjectByType<Assets.Controller.PieceMover>();
                }
                
                if (pieceMover != null)
                {
                    // Move the piece using PieceMover
                    pieceMover.MovePiece(selectedPiece, targetPosition);
                }
                else
                {
                    // Fallback: Move the piece directly
                    Debug.LogWarning("PieceMover not found! Moving piece directly.");
                    Vector2 newPosition = board.GetPosition(targetPosition);
                    selectedPiece.GameObject.transform.position = new Vector3(newPosition.x, newPosition.y, selectedPiece.GameObject.transform.position.z);
                    Debug.Log($"Moved piece directly to {targetPosition}");
                }
                
                // Unselect the piece after moving
                GameManager.Instance.UnselectCurrentPiece();
                
                // Advance to the next player's turn
                Assets.Controller.TurnManager turnManager = FindFirstObjectByType<Assets.Controller.TurnManager>();
                if (turnManager != null)
                {
                    turnManager.NextTurn();
                    Debug.Log($"Advanced to next player's turn. Current player: {turnManager.currentPlayer}");
                }
                else
                {
                    Debug.LogError("TurnManager not found! Cannot advance to next player's turn.");
                }
            }
            else
            {
                Debug.Log("No piece selected");
            }
        }
        else
        {
            Debug.Log("No valid board position found near click");
        }
    }

    private string FindClosestBoardPosition(Vector3 clickPosition)
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
            float distance = Vector2.Distance(new Vector2(clickPosition.x, clickPosition.y), positionVector);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
        }
        
        // Only return if the closest position is within a reasonable distance
        // Increased threshold to 2.0 units to make it easier to click
        if (closestDistance <= 2.0f)
        {
            Debug.Log($"Found position {closestPosition} at distance {closestDistance}");
            return closestPosition;
        }
        
        Debug.Log($"No position found within threshold. Closest was {closestPosition} at distance {closestDistance}");
        
        return null;
    }
}
