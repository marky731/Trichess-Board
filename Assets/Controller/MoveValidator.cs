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

        // MoveValidator sınıfı, geçerli hamlelerin kontrolünü sağlamak için Chessboard objesini alır
        public MoveValidator(Chessboard chessboard)
        {
            _chessboard = chessboard ?? throw new ArgumentNullException(nameof(chessboard));
        }

        // Hamlenin geçerli olup olmadığını kontrol eder
        public bool IsValidMove(Piece piece, Move move)
        {
            // Eğer hamlede kaynak ya da hedef boşsa geçersiz say
            if (move == null || move.Source == null || move.Destination == null)
                return false;

            // Eğer taş geçersizse (null) hamleyi geçersiz say
            if (piece == null)
                return false;

            // Taşın hareket kurallarına uygun olup olmadığını kontrol et
            if (!IsMoveAllowedByPiece(piece, move.Source, move.Destination))
                return false;

            // Yolun açık olup olmadığını kontrol et (Vezir, Fil, Kale için)
            if (!IsPathClear(move.Source, move.Destination))
                return false;

            // Hamlenin tahtanın sınırları içinde olup olmadığını kontrol et
            if (!IsMoveWithinBounds(move))
                return false;

            // Oyuncunun kendi taşını hareket ettirdiğinden emin ol
            if (!IsPlayerMovingOwnPiece(move))
                return false;

            // Eğer özel kurallar uygulanıyorsa burada kontrol et (Örneğin: Şah tehdit altında mı)
            if (IsKingInCheckAfterMove(move))
                return false;

            // Eğer yukarıdaki kontrollerin hepsi geçerli ise hamle geçerli sayılır
            return true;
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
