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
            Debug.Log($"Clicked on piece: {piece.GetType().Name}");
            
            // Tell the GameManager this piece was clicked
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SelectPiece(piece);
            }
            else
            {
                Debug.LogError("GameManager.Instance is null!");
            }
        }
        else
        {
            Debug.LogError("Piece reference is null in PieceController.OnMouseDown!");
        }
    }
}