using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public class WhitePawn : Pawn
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            Direction.WhiteForward // Use the predefined direction for white pawns
        };

        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            Direction.WhiteForwardRight, // Diagonal right for capturing
            Direction.WhiteForwardLeft   // Diagonal left for capturing
        };

        public WhitePawn(GameObject gameObject, int playerId)
            : base(PieceColor.White, gameObject, playerId) { }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        public override HashSet<Direction> GetTakingDirections()
        {
            return TAKING_DIRECTIONS;
        }

        public override List<string> GetPossibleMoves(Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu al

            if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

            foreach (Direction direction in GetDirections())
            {
                Field nextField = currentField;
                while (true)
                {
                    nextField = direction.Move(nextField);

                    if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                        break;

                    possibleMoves.Add($"{nextField.X},{nextField.Y}");

                    if (board.IsOccupied(nextField))
                        break;
                }
            }

            return possibleMoves;
        }
    }
}
