using System.Collections.Generic;
using UnityEngine;

namespace Assets.Model.ChessboardMain.Pieces
{
    namespace Assets.Model.ChessboardMain.Pieces
    {
        public class Rook : Piece
        {
            private static readonly HashSet<Direction> DIRECTIONS = (HashSet<Direction>)StraightDirections.Get();

            public Rook(PieceColor color, GameObject gameObject, int playerId) : base(color, true, gameObject, playerId) { }

            public override string GetAbbreviation()
            {
                return "R";
            }

            public override HashSet<Direction> GetDirections()
            {
                return DIRECTIONS;
            }
            // GetPossibleMoves metodunu ekleyelim
            public override List<string> GetPossibleMoves(Board board)
            {
                List<string> possibleMoves = new List<string>();
                Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu al

                if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

                foreach (Direction direction in GetDirections())
                {
                    Field nextField = currentField; // Mevcut konumdan başla
                    while (true)
                    {
                        nextField = direction.Move(nextField); // Yöne göre ilerle

                        if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                            break;

                        possibleMoves.Add($"{nextField.X},{nextField.Y}"); // Field.Position yerine koordinat stringi ekledik

                        if (board.IsOccupied(nextField)) // Bir taş varsa dur
                            break;
                    }
                }

                return possibleMoves;
            }

        }
    }
}
