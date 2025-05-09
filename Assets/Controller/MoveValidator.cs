//Bu sınıf, verilen bir hamlenin geçerli olup olmadığını kontrol eder.
//Taşın hareket kurallarına uygun olup olmadığı, yolun açık olup olmadığı,
//tahtanın sınırları içinde olup olmadığı,
//oyuncunun kendi taşını hareket ettirip ettirmediği gibi çeşitli kontrol noktalarını geçtikten sonra,
//hamlenin geçerli olup olmadığına karar verir.
//Ayrıca, hamle sonrası Şah'ın tehdit altında olup olmadığını da kontrol eder.

using Assets.Model.ChessboardMain.Pieces;
using Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using Assets.Model.ChessboardMain;
using Direction = Assets.Model.ChessboardMain.Direction;
using UnityEngine;

namespace Assets.Controller
{
    public class MoveValidator : MonoBehaviour
    {
        // Tahtayı temsil eden değişken
        private Chessboard _chessboard;

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            Debug.Log("MoveValidator Awake called.");
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("MoveValidator Start called.");
            
            // Check if _chessboard is null
            if (_chessboard == null)
            {
                Debug.LogWarning("Chessboard is null in MoveValidator. Move validation will not work properly.");
            }
        }

        // Set the Chessboard reference after instantiation
        public void SetChessboard(Chessboard chessboard)
        {
            _chessboard = chessboard ?? throw new ArgumentNullException(nameof(chessboard));
            Debug.Log("Chessboard set in MoveValidator.");
        }

        // Hamlenin geçerli olup olmadığını kontrol eder
        public bool IsValidMove(Piece piece, Move move)
        {
            // Check if Chessboard is initialized
            if (_chessboard == null)
            {
                Debug.LogError("Chessboard is null in MoveValidator.IsValidMove! Move validation will fail.");
                return false; // Return false to prevent the move when Chessboard is null
            }

            // Eğer hamlede kaynak ya da hedef boşsa geçersiz say
            if (move == null || move.Source == null || move.Destination == null)
            {
                Debug.LogWarning("Move, Source, or Destination is null in IsValidMove!");
                return false;
            }

            // Eğer taş geçersizse (null) hamleyi geçersiz say
            if (piece == null)
            {
                Debug.LogWarning("Piece is null in IsValidMove!");
                return false;
            }

            try
            {
                Debug.Log($"Validating move from ({move.Source.X}, {move.Source.Y}) to ({move.Destination.X}, {move.Destination.Y}) for piece {piece.GetType().Name}");
                
                // Check if the piece is trying to move to its current position
                if (Math.Abs(move.Source.X - move.Destination.X) < 0.01 && Math.Abs(move.Source.Y - move.Destination.Y) < 0.01)
                {
                    Debug.Log("Validation failed: Piece is trying to move to its current position");
                    return false;
                }
                
                // Taşın hareket kurallarına uygun olup olmadığını kontrol et
                if (!IsMoveAllowedByPiece(piece, move.Source, move.Destination))
                {
                    Debug.Log($"Validation failed: Move is not allowed by {piece.GetType().Name} movement rules");
                    return false;
                }
                else
                {
                    Debug.Log("Move is allowed by piece movement rules");
                }

                // Yolun açık olup olmadığını kontrol et (Vezir, Fil, Kale için)
                if (!IsPathClear(move.Source, move.Destination))
                {
                    Debug.Log("Validation failed: Path is not clear");
                    return false;
                }
                else
                {
                    Debug.Log("Path is clear");
                }

                // Hamlenin tahtanın sınırları içinde olup olmadığını kontrol et
                if (!IsMoveWithinBounds(move))
                {
                    Debug.Log("Validation failed: Move is not within bounds");
                    return false;
                }
                else
                {
                    Debug.Log("Move is within bounds");
                }

                // Oyuncunun kendi taşını hareket ettirdiğinden emin ol
                if (!IsPlayerMovingOwnPiece(move))
                {
                    Debug.Log($"Validation failed: Player is not moving their own piece. Piece color: {move.MovedPiece.GetColor()}, Current player: {_chessboard.CurrentPlayerColor}");
                    return false;
                }
                else
                {
                    Debug.Log("Player is moving their own piece");
                }

                // Eğer özel kurallar uygulanıyorsa burada kontrol et (Örneğin: Şah tehdit altında mı)
                if (IsKingInCheckAfterMove(move))
                {
                    Debug.Log("Validation failed: King would be in check after move");
                    return false;
                }
                else
                {
                    Debug.Log("King would not be in check after move");
                }

                // Eğer yukarıdaki kontrollerin hepsi geçerli ise hamle geçerli sayılır
                Debug.Log("All validation checks passed - move is valid");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in IsValidMove: {e.Message}\n{e.StackTrace}");
                return false; // Return false to prevent the move when there's an error
            }
        }

        // Taşın hareket kurallarına uygun olup olmadığını kontrol eder
        private bool IsMoveAllowedByPiece(Piece piece, Field source, Field destination)
        {
            // Get the piece's allowed directions
            HashSet<Direction> allowedDirections = piece.GetDirections();
            
            // Calculate the move direction
            Direction moveDirection = Direction.GetDirection(source, destination);
            
            // For debugging
            Debug.Log($"Move direction: ({moveDirection.X}, {moveDirection.Y})");
            
            // Check if any allowed direction is close enough to the move direction
            foreach (var allowedDir in allowedDirections)
            {
                // Calculate the dot product to check direction similarity
                double dotProduct = moveDirection.X * allowedDir.X + moveDirection.Y * allowedDir.Y;
                double moveDirLength = Math.Sqrt(moveDirection.X * moveDirection.X + moveDirection.Y * moveDirection.Y);
                double allowedDirLength = Math.Sqrt(allowedDir.X * allowedDir.X + allowedDir.Y * allowedDir.Y);
                
                // Avoid division by zero
                if (moveDirLength < 0.01 || allowedDirLength < 0.01)
                    continue;
                    
                double similarity = dotProduct / (moveDirLength * allowedDirLength);
                
                // If the directions are similar enough (cos of angle > 0.8, which is about 36 degrees)
                if (similarity > 0.8)
                {
                    Debug.Log($"Direction ({moveDirection.X}, {moveDirection.Y}) matches allowed direction ({allowedDir.X}, {allowedDir.Y}) with similarity {similarity}");
                    return true;
                }
            }
            
            Debug.Log($"No allowed direction matches move direction ({moveDirection.X}, {moveDirection.Y})");
            return false;
        }

        // Yolun açık olup olmadığını kontrol eder (Vezir, Fil, Kale gibi taşlar için)
        private bool IsPathClear(Field source, Field destination)
        {
            // Kaynak ve hedef arasında yolun hangi alanlarda olduğunu alır
            List<Field> path = _chessboard.GetPath(source, destination);
            // Yol üzerindeki her kareyi kontrol eder
            foreach (Field field in path)
            {
                // Eğer yol üzerindeki bir karede taş varsa, yol kapanmış demektir
                if (field.OccupiedPiece != null)
                {
                    return false;
                }
            }
            return true; // Yol açıksa hamle geçerlidir
        }

        // Hamlenin tahtanın sınırları içinde olup olmadığını kontrol eder
        private bool IsMoveWithinBounds(Move move)
        {
            // Hedef alan tahtanın sınırları içinde mi?
            return _chessboard.IsWithinBounds(move.Destination);
        }

        // Oyuncunun kendi taşını hareket ettirdiğinden emin olur
        private bool IsPlayerMovingOwnPiece(Move move)
        {
            // First check if the piece's PlayerId matches the current player
            if (move.MovedPiece.PlayerId > 0)
            {
                int currentPlayer = 1; // Default to player 1
                
                // Try to get the current player from TurnManager.Instance
                if (TurnManager.Instance != null)
                {
                    currentPlayer = TurnManager.Instance.currentPlayer;
                    Debug.Log($"Using TurnManager.Instance.currentPlayer: {currentPlayer}");
                }
                // Fallback to _chessboard.CurrentPlayerColor
                else if (_chessboard != null)
                {
                    // Convert color to player ID (assuming White=1, Gray=2, Black=3)
                    if (_chessboard.CurrentPlayerColor == Assets.Model.PieceColor.White)
                        currentPlayer = 1;
                    else if (_chessboard.CurrentPlayerColor == Assets.Model.PieceColor.Gray)
                        currentPlayer = 2;
                    else if (_chessboard.CurrentPlayerColor == Assets.Model.PieceColor.Black)
                        currentPlayer = 3;
                    
                    Debug.Log($"Using _chessboard.CurrentPlayerColor: {_chessboard.CurrentPlayerColor} -> Player {currentPlayer}");
                }
                
                bool isValid = move.MovedPiece.PlayerId == currentPlayer;
                Debug.Log($"IsPlayerMovingOwnPiece: Piece PlayerId={move.MovedPiece.PlayerId}, Current Player={currentPlayer}, IsValid={isValid}");
                return isValid;
            }
            
            // Fallback to color check if PlayerId is not set
            Debug.LogWarning("Piece PlayerId not set, falling back to color check");
            return move.MovedPiece.GetColor() == _chessboard.CurrentPlayerColor;
        }

        // Eğer hamle sonrası Şah tehdit altında kalacaksa, hamleyi geçersiz sayar
        private bool IsKingInCheckAfterMove(Move move)
        {
            // Eğer hamle sonrası Şah tehdit altına giriyorsa, hamleyi geçersiz kılar
            return _chessboard.WouldKingBeInCheckAfterMove(move);
        }
    }
}
