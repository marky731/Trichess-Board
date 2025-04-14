using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System.Collections.Generic;
using UnityEngine;  // GameObject için gerekli

namespace Assets.Model.ChessboardMain.Pieces
{
    public class Bishop : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = DiagonalDirections.Get();

        // Düzgün constructor: GameObject ve playerId eklenerek base çağrısı yapıldı
        public Bishop(PieceColor color, GameObject gameObject, int playerId)
            : base(color, true, gameObject, playerId) { }

        public override string GetAbbreviation()
        {
            return "B";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        // Eksik olan GetPossibleMoves metodunu ekledik
        public override List<string> GetPossibleMoves(Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu Field olarak al

            if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

            foreach (Direction direction in GetDirections())
            {
                Field nextField = currentField; // Mevcut konumdan başla
                while (true)
                {
                    nextField = direction.Move(nextField); // Yöne göre ilerle

                    if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                        break;

                    possibleMoves.Add(nextField.Position); // `Field.Position` kullanarak string ekle

                    if (board.IsOccupied(nextField)) // Bir taş varsa dur
                        break;
                }
            }

            return possibleMoves;
        }

    }
}
