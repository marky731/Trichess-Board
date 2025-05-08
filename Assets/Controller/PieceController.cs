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
            
            // Check if it's this player's turn
            int currentPlayerId = GameManager.Instance.CurrentPlayerId;
            if (piece.PlayerId != currentPlayerId)
            {
                Debug.Log($"Not your turn! Current player: {currentPlayerId}, Piece belongs to player: {piece.PlayerId}");
                return;
            }
            
            Debug.Log($"Selected piece: {piece.GetType().Name} (Player {piece.PlayerId})");
            
            // Tell the GameManager this piece was clicked
            GameManager.Instance.SelectPiece(piece);
        }
        else
        {
            Debug.LogError("Piece reference is null in PieceController.OnMouseDown!");
        }
    }
}
