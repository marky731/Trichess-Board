using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Model.ChessboardMain.Pieces
{
    public class King : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>();

        static King()
        {
            DIRECTIONS.UnionWith(DiagonalDirections.Get());
            DIRECTIONS.UnionWith(StraightDirections.Get());
        }

        public King(PieceColor color, GameObject gameObject, int playerId)
    : base(color, false, gameObject, playerId) { }


        public override string GetAbbreviation()
        {
            return "K";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        public override List<string> GetPossibleMoves(Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu al

            if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

            foreach (Direction direction in GetDirections()) // King tüm yönlere hareket edebilir
            {
                Field nextField = direction.Move(currentField); // Bir kare ilerle

                if (nextField == null) continue; // Tahta dışına çıkarsa geç

                // Eğer hamle geçerliyse ve mevcut oyuncuya ait değilse ekle
                if (board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                {
                    possibleMoves.Add($"{nextField.X},{nextField.Y}");
                }
            }

            return possibleMoves;
        }

    }
}
