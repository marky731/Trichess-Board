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
            // Use the singleton instance if available
            turnManager = TurnManager.Instance;
            if (turnManager == null)
            {
                turnManager = FindFirstObjectByType<TurnManager>();
                Debug.LogWarning("TurnManager.Instance was null, found instance through FindFirstObjectByType");
            }
            board = FindFirstObjectByType<Board>();
            
            Debug.Log("PieceMover initialized");
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
                // If piece.position is null or empty, try to find the current position
                if (string.IsNullOrEmpty(piece.position))
                {
                    Debug.LogWarning("Piece position is null or empty. Trying to determine current position...");
                    
                    // Find the current position based on the GameObject's transform
                    string currentPosition = FindClosestBoardPosition(board, piece.GameObject.transform.position);
                    if (currentPosition != null)
                    {
                        // Update the piece's position property
                        piece.position = currentPosition;
                        Debug.Log($"Updated piece position to: {currentPosition}");
                    }
                    else
                    {
                        Debug.LogError("Could not determine the current position of the piece!");
                        return;
                    }
                }
                
                sourceField = board.GetField(piece.position);
                Debug.Log($"Source field: {(sourceField != null ? piece.position : "null")}");
                
                if (sourceField == null)
                {
                    Debug.LogError($"Source field is null for position: {piece.position}");
                    return;
                }
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
                
                if (targetField == null)
                {
                    Debug.LogError($"Target field is null for position: {targetPosition}");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting target field: {e.Message}");
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
            // Enable move validation for proper chess rules
            bool skipValidation = false; // Validation is now enabled
            if (moveValidator != null && !skipValidation)
            {
                try
                {
                    Debug.Log($"Validating move from {piece.position} to {targetPosition}...");
                    if (!moveValidator.IsValidMove(piece, move))
                    {
                        Debug.LogError($"Invalid move from {piece.position} to {targetPosition}! The move does not comply with chess rules.");
                        
                        // Prevent invalid moves
                        return; // This line is now uncommented to prevent invalid moves
                    }
                    else
                    {
                        Debug.Log($"Move from {piece.position} to {targetPosition} is valid.");
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
                    Debug.LogWarning("MoveValidator is null! Trying to find it...");
                    moveValidator = FindFirstObjectByType<MoveValidator>();
                    
                    if (moveValidator == null)
                    {
                        Debug.LogError("Could not find MoveValidator component! Creating one...");
                        GameObject validatorObj = new GameObject("MoveValidator");
                        moveValidator = validatorObj.AddComponent<MoveValidator>();
                        
                        // Try to initialize the MoveValidator
                        Board boardComponent = FindFirstObjectByType<Board>();
                        if (boardComponent != null)
                        {
                            Debug.Log("Found Board component. Waiting for it to initialize MoveValidator...");
                        }
                        else
                        {
                            Debug.LogError("Could not find Board component! Move validation will not work properly.");
                        }
                    }
                }
                
                if (skipValidation)
                {
                    Debug.LogWarning("Validation is skipped. This should be enabled in production.");
                }
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

            // Track if the move was successful
            bool moveSuccessful = true;
            
            // 7. Sıradaki oyuncuya geçiş yapılır - only if the move was successful
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
                Debug.Log($"Found position {closestPosition} at distance {closestDistance}");
                return closestPosition;
            }
            
            Debug.Log($"No position found within threshold. Closest was {closestPosition} at distance {closestDistance}");
            
            return null;
        }
    }
}
