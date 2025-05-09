using UnityEngine;
using Assets.Model.ChessboardMain.Pieces;

[RequireComponent(typeof(Collider2D))]
public class PieceController : MonoBehaviour
{
    private Piece piece;

    void Start()
    {
        piece = GetComponent<Piece>();
        
        if (piece == null)
        {
            Debug.LogError("No Piece component found on this GameObject");
        }
        else
        {
            // Ensure the GameObject property is set
            piece.GameObject = gameObject;
        }
    }

    void OnMouseDown()
    {
        if (piece != null)
        {
            Debug.Log($"Clicked on piece: {piece.GetType().Name} (Player {piece.PlayerId})");
            
            // Check if GameManager exists
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return;
            }
            
            // Check if it's this player's turn using TurnManager.Instance if available
            int currentPlayerId;
            if (Assets.Controller.TurnManager.Instance != null)
            {
                currentPlayerId = Assets.Controller.TurnManager.Instance.currentPlayer;
            }
            else
            {
                currentPlayerId = GameManager.Instance.CurrentPlayerId;
            }
            
            if (piece.PlayerId != currentPlayerId)
            {
                Debug.Log($"Not your turn! Current player: {currentPlayerId}, Piece belongs to player: {piece.PlayerId}");
                return;
            }
            
            Debug.Log($"Valid piece selection: Current player: {currentPlayerId}, Piece belongs to player: {piece.PlayerId}");
            
            // Find the current position of the piece based on its GameObject position
            Board board = FindFirstObjectByType<Board>();
            if (board != null)
            {
                string currentPosition = FindClosestBoardPosition(board, transform.position);
                if (currentPosition != null)
                {
                    // Update the piece's position property
                    piece.position = currentPosition;
                    Debug.Log($"Updated piece position to: {currentPosition}");
                }
                else
                {
                    Debug.LogWarning("Could not determine the current position of the piece!");
                }
            }
            else
            {
                Debug.LogError("Board not found! Cannot determine piece position.");
            }
            
            Debug.Log($"Selected piece: {piece.GetType().Name} (Player {piece.PlayerId}) at position {piece.position}");
            
            // Tell the GameManager this piece was clicked
            GameManager.Instance.SelectPiece(piece);
        }
        else
        {
            Debug.LogError("Piece reference is null in PieceController.OnMouseDown!");
        }
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
            Debug.Log($"Found position {closestPosition} at distance {closestDistance}");
            return closestPosition;
        }
        
        Debug.Log($"No position found within threshold. Closest was {closestPosition} at distance {closestDistance}");
        
        return null;
    }
}
