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
                // Check if the piece is trying to move to its current position
                if (Math.Abs(move.Source.X - move.Destination.X) < 0.01 && Math.Abs(move.Source.Y - move.Destination.Y) < 0.01)
                {
                    Debug.Log($"Piece is trying to move to its current position ({move.Source.X}, {move.Source.Y}). This is not a valid move.");
                    return false;
                }
                
                // Taşın hareket kurallarına uygun olup olmadığını kontrol et
                if (!IsMoveAllowedByPiece(piece, move.Source, move.Destination))
                {
                    Debug.Log($"Move from {move.Source.Position} to {move.Destination.Position} is not allowed by piece {piece.GetType().Name}");
                    return false;
                }

                // Yolun açık olup olmadığını kontrol et (Vezir, Fil, Kale için)
                if (!IsPathClear(move.Source, move.Destination))
                {
                    Debug.Log($"Path from {move.Source.Position} to {move.Destination.Position} is not clear");
                    return false;
                }

                // Hamlenin tahtanın sınırları içinde olup olmadığını kontrol et
                if (!IsMoveWithinBounds(move))
                {
                    Debug.Log($"Move to {move.Destination.Position} is not within bounds");
                    return false;
                }

                // Oyuncunun kendi taşını hareket ettirdiğinden emin ol
                if (!IsPlayerMovingOwnPiece(move))
                {
                    Debug.Log($"Player is not moving their own piece. Piece color: {move.MovedPiece.GetColor()}, Current player: {_chessboard.CurrentPlayerColor}");
                    return false;
                }

                // Eğer özel kurallar uygulanıyorsa burada kontrol et (Örneğin: Şah tehdit altında mı)
                if (IsKingInCheckAfterMove(move))
                {
                    Debug.Log("King would be in check after this move");
                    return false;
                }

                // Eğer yukarıdaki kontrollerin hepsi geçerli ise hamle geçerli sayılır
                Debug.Log($"Move from {move.Source.Position} to {move.Destination.Position} is valid");
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
            // Taşın izin verilen yönlerini alır
            HashSet<Direction> allowedDirections = piece.GetDirections();
            // Hareketin yönünü hesaplar
            Direction moveDirection = Direction.GetDirection(source, destination);
            // Eğer taşın yönü izin verilen yönlerden biriyse hamle geçerlidir
            return allowedDirections.Contains(moveDirection);
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
            // Eğer taşın rengi, oyuncunun rengiyle uyuyorsa geçerli
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
