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


namespace Assets.Model
{
    public class MoveValidator
    {
        private Chessboard _chessboard;

        public MoveValidator(Chessboard chessboard)
        {
            _chessboard = chessboard ?? throw new ArgumentNullException(nameof(chessboard));
        }

        /// <summary>
        /// Hamlenin geçerli olup olmadığını kontrol eder.
        /// </summary>
        public bool IsValidMove(Move move)

        {
            if (move == null || move.Source == null || move.Destination == null)
                return false;

            Piece piece = move.MovedPiece;

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

            // Eğer özel kurallar uygulanıyorsa burada kontrol et
            if (IsKingInCheckAfterMove(move))
                return false;

            return true;
        }

        private bool IsMoveAllowedByPiece(Piece piece, Field source, Field destination)
        {
            HashSet<Direction> allowedDirections = piece.GetDirections();
            Direction moveDirection = Direction.GetDirection(source, destination);
            return allowedDirections.Contains(moveDirection);
        }


        private bool IsPathClear(Field source, Field destination)
        {
            // Yol üzerindeki taşları kontrol et
            List<Field> path = _chessboard.GetPath(source, destination);
            foreach (Field field in path)
            {
                if (field.OccupiedPiece != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsMoveWithinBounds(Move move)
        {
            return _chessboard.IsWithinBounds(move.Destination);
        }

        private bool IsPlayerMovingOwnPiece(Move move)
        {
            return move.MovedPiece.GetColor() == _chessboard.CurrentPlayerColor;
        }

        private bool IsKingInCheckAfterMove(Move move)
        {
            // Şah tehdit altında kalacaksa geçersiz hamle
            return _chessboard.WouldKingBeInCheckAfterMove(move);
        }
    }
}
