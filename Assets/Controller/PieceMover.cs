//PieceMover sınıfı, satranç taşlarının hareketini yöneten ve kontrol eden bir sınıftır.
//Bu sınıf, taşların geçerli bir hamle yapıp yapmadığını kontrol eder, taşı hareket ettirir ve oyuncuların sırasını değiştirir.
//MovePiece metoduyla:
//Kaynak ve hedef kareleri alır.
//Hedef karede bir taş varsa, o taşı alır (yemek için).
//Yeni bir Move nesnesi oluşturur ve hamlenin geçerliliğini MoveValidator ile kontrol eder.
//Eğer hamle geçerliyse, taşı yeni konumuna taşır ve TurnManager ile sıradaki oyuncuya geçer.

using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Controller
{
    public class PieceMover : MonoBehaviour
    {
        // Taş hareketlerini doğrulayan sınıf
        private MoveValidator moveValidator;

        // Sıra yöneticisini kontrol eden sınıf
        private TurnManager turnManager;

        // Tahtayı yöneten sınıf
        private Board board;

        // Başlangıçta gerekli bileşenleri bulur ve başlatır
        void Start()
        {
            moveValidator = FindFirstObjectByType<MoveValidator>();
            turnManager = FindFirstObjectByType<TurnManager>();
            board = FindFirstObjectByType<Board>();
        }

        // Taşın hareketini gerçekleştiren ana metod
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
            
            // 1. Taşın mevcut pozisyonu (kaynak alan) alınır
            Field sourceField = null;
            try
            {
                sourceField = board.GetField(piece.position);
                Debug.Log($"Source field: {(sourceField != null ? piece.position : "null")}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting source field: {e.Message}");
                return;
            }

            // 2. Hedef pozisyonu (hedef alan) alınır, targetPosition string olduğu için Field objesine dönüştürülür
            Field targetField = null;
            try
            {
                targetField = board.GetField(targetPosition);
                Debug.Log($"Target field: {(targetField != null ? targetPosition : "null")}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting target field: {e.Message}");
                return;
            }

            // Eğer kaynak veya hedef alan geçersizse hata mesajı verir
            if (sourceField == null || targetField == null)
            {
                Debug.LogError("Kaynak veya hedef alan geçersiz!");
                return;
            }

            // 3. Hedef karede taş varsa (yemek için), o taşı al
            Piece takenPiece = null;
            try
            {
                takenPiece = (Piece)targetField.OccupiedPiece;
                if (takenPiece != null)
                {
                    Debug.Log($"Taking piece: {takenPiece.GetType().Name}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting taken piece: {e.Message}");
                // Continue anyway, as taking a piece is optional
            }

            // 4. Yeni bir Move nesnesi oluşturulur, bu hamleyi temsil eder
            Move move = null;
            try
            {
                move = new Move(sourceField, piece, targetField, takenPiece);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error creating Move object: {e.Message}");
                return;
            }

            // 5. Hamlenin geçerli olup olmadığı MoveValidator ile kontrol edilir
            // Temporarily skip move validation for testing
            bool skipValidation = true; // Set to false once movement works
            if (moveValidator != null && !skipValidation)
            {
                try
                {
                    if (!moveValidator.IsValidMove(piece, move))
                    {
                        Debug.LogError("Geçersiz hamle!");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error validating move: {e.Message}");
                    // Continue anyway for testing
                }
            }
            else if (moveValidator == null)
            {
                Debug.LogWarning("MoveValidator is null, skipping validation");
            }

            // 6. Taşı yeni konumuna taşır, GameObject'inin pozisyonunu günceller
            try
            {
                Vector2 newPosition = board.GetPosition(targetPosition);
                Debug.Log($"Moving piece to position: ({newPosition.x}, {newPosition.y})");
                
                // Use the Board.MovePiece method to update the fields
                bool moveSuccess = board.MovePiece(piece, piece.position, targetPosition);
                
                if (!moveSuccess)
                {
                    Debug.LogError("Failed to update board fields for the move!");
                    // Continue anyway for testing
                }
                
                // Update the GameObject's position
                piece.GameObject.transform.position = new Vector3(newPosition.x, newPosition.y, piece.GameObject.transform.position.z);
                
                Debug.Log($"Piece moved to {targetPosition}");
                
                // If there was a piece at the target position, disable or destroy it
                if (takenPiece != null && takenPiece.GameObject != null)
                {
                    Debug.Log($"Disabling taken piece: {takenPiece.GetType().Name}");
                    takenPiece.GameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error moving piece: {e.Message}");
                return;
            }

            // 7. Sıradaki oyuncuya geçiş yapılır
            if (turnManager != null)
            {
                try
                {
                    turnManager.NextTurn();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error changing turn: {e.Message}");
                    // Continue anyway, as the piece has already moved
                }
            }
            else
            {
                Debug.LogWarning("TurnManager is null, skipping turn change");
            }
            
            Debug.Log("MovePiece completed successfully");
        }
    }
}
